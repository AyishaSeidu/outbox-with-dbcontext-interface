using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OutboxWithDbContextInterface.Models;

namespace OutboxWithDbContextInterface.Data;

public interface IMyDbContext
{
    DbSet<Child> Children { get; }

    DbSet<Parent> Parents { get; }

    Task<int> SaveChangesAsync();

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    public void Update<TEntity>(TEntity entity) where TEntity : notnull;
}