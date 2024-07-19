using Azure;
using MassTransit;
using MassTransit.AzureServiceBusTransport.Configuration;
using Microsoft.EntityFrameworkCore;
using OutboxWithDbContextInterface.Data;
using OutboxWithDbContextInterface.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// When I switch this order of registration, my message ends up on the queue. So it seems the outbox tables are only added to the first db context interface registered 
builder.Services.AddDbContext<IAnotherDbContext, DbContextImplementation>();
builder.Services.AddDbContext<IMyDbContext, DbContextImplementation>();

builder.Services.AddScoped<IFamilyRepo, FamilyRepo>();

builder.Services.AddMassTransit(x =>
{
    var host = ""; //TODO
    var sharedAccessKey = ""; //TODO;
    var sharedAccessKeyName = ""; //TODO
    x.AddEntityFrameworkOutbox<DbContextImplementation>(cfg =>
    {
        cfg.DuplicateDetectionWindow =
            TimeSpan.FromMinutes(10);
        cfg.UsePostgres();
        cfg.UseBusOutbox();
    });

    x.UsingAzureServiceBus((context, cfg) =>
    {
        var hostSettings = new HostSettings
        {
            ServiceUri = new Uri(host),
            NamedKeyCredential = new AzureNamedKeyCredential(sharedAccessKeyName, sharedAccessKey)
        };
        cfg.Host(hostSettings);
        cfg.AutoDeleteOnIdle = TimeSpan.FromMinutes(5);
        cfg.EnableDuplicateDetection(TimeSpan.FromMinutes(5));
        cfg.ConcurrentMessageLimit = 3;
        cfg.UseMessageRetry(r => r.Intervals(100, 500, 1000, 1000, 1000));
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DbContextImplementation>();
    context.Database.Migrate();
}

app.Run();