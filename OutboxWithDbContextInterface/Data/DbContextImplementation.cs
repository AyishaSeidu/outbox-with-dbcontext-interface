using MassTransit;
using Microsoft.EntityFrameworkCore;
using OutboxWithDbContextInterface.Models;

namespace OutboxWithDbContextInterface.Data;

public class DbContextImplementation : DbContext, IMyDbContext, IAnotherDbContext
{
    public DbSet<SomeModel> SomeModels => Set<SomeModel>();
    public DbSet<Child> Children => Set<Child>();
    public DbSet<Parent> Parents => Set<Parent>();

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : notnull
    {
        base.Update(entity);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var connectionString =
            "host=localhost;user id=postgres;password=<PASSWORD>;database=HttpOutbox;";

        optionsBuilder
            .UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextImplementation).Assembly);

        // Add Mass Transport entities
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}