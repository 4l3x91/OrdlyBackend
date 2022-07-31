namespace OrdlyBackend.Infrastructure;

using OrdlyBackend.Models;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly OrdlyContext _context;
    private readonly DbSet<T> _entities;

    public Repository(OrdlyContext context)
    {
        _context = context;
        _entities = context.Set<T>();
        _context.SavingChanges += Context_SavingChanges;

    }
    public async Task<T> AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        _entities.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<T> GetLastAsync()
    {
        return await _entities.OrderBy(x => x.RowCreated).LastAsync();
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var idProperty = entity.GetType().GetProperty("Id").GetValue(entity, null);
        var oldEntity = await _entities.FindAsync(idProperty);
        _context.ChangeTracker.Clear();

        SetRowData(entity, oldEntity);
        _entities.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<T> GetByIdAsync(int Id)
    {
        return await _entities.FindAsync(Id);
    }
    
    public async Task<T> GetByUserIdAsync(int id)
    {
        var e = await _entities.Where(x => _entities.GetType().GetProperty("UserId").GetValue(x, null).ToString() == id.ToString()).ToListAsync();
        return e.FirstOrDefault();
    }

    private void Context_SavingChanges(object sender, SavingChangesEventArgs args)
    {
        var entities = _context.ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && x.Entity is T &&
            (x.State == EntityState.Added || x.State == EntityState.Modified))
            .ToArray();

        foreach (var entity in entities)
        {
            var baseEntity = entity.Entity as BaseEntity;

            if (entity.State == EntityState.Added)
            {
                baseEntity.RowCreated = DateTime.Now;
            }

            baseEntity.RowModified = DateTime.Now;
            baseEntity.RowVersion += 1;
        }
    }

    private void SetRowData(T entity, T oldEntity)
    {
        entity.RowCreated = oldEntity.RowCreated;
        entity.RowModified = oldEntity.RowModified;
        entity.RowVersion = oldEntity.RowVersion;
    }
}
