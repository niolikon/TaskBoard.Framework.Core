using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Exceptions.Persistence;

namespace TaskBoard.Framework.Core.Repositories;

public class BaseCrudRepository<TEntity, TId> : ICrudRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
{
    protected DbContext _dbContext;

    public BaseCrudRepository(DbContext appDbContext)
    {
        _dbContext = appDbContext;
    }
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        EntityEntry<TEntity> result = await _dbContext.Set<TEntity>().AddAsync(entity);
        
        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("CreateAsync created no rows");
        }

        return result.Entity;
    }

    public async Task<IEnumerable<TEntity>> ReadAllAsync()
    {
        return await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> ReadAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TEntity> ReadAsync(TId id)
    {
        TEntity? entityInDb = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entityInDb == null)
        {
            throw new EntityNotFoundException($"Could not find {typeof(TEntity)} with id {id}");
        }

        return entityInDb;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        TEntity entityInDatabase = await ReadAsync(entity.Id);
        entityInDatabase.CopyFrom(entity);

        _dbContext.Set<TEntity>().Update(entityInDatabase);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("UpdateAsync updated no rows");
        }

        return entityInDatabase;
    }

    public async Task DeleteAsync(TId id)
    {
        TEntity entityInDatabase = await ReadAsync(id);

        _dbContext.Set<TEntity>().Remove(entityInDatabase);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1) 
        {
            throw new RepositorySaveChangeFailedException("DeleteAsync deleted no rows");
        }
    }
}
