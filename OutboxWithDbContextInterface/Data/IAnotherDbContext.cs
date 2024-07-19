using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OutboxWithDbContextInterface.Models;

namespace OutboxWithDbContextInterface.Data;

public interface IAnotherDbContext
{
    DbSet<SomeModel> SomeModels { get; }

    //and other models

    Task<int> SaveChangesAsync();

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    public void Update<TEntity>(TEntity entity) where TEntity : notnull;
}