using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Exceptions.Persistence;

namespace TaskBoard.Framework.Core.Repositories;

public class BaseSecuredCrudRepositoryPersistedOwner<TEntity, TId, TOwner> : ISecuredCrudRepository<TEntity, TId, TOwner>
    where TEntity : BaseOwnedEntity<TId, TOwner>
    where TOwner : PersistedOwnerEntity, new()
{
    protected DbContext _dbContext;
    protected UserManager<TOwner> _userManager;

    public BaseSecuredCrudRepositoryPersistedOwner(IdentityDbContext<TOwner> dbContext, UserManager<TOwner> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<TOwner> GetOwnerAsync(string id)
    {
        return await _userManager.FindByIdAsync(id) ??
            throw new EntityOwnershipViolationException($"Could not find owner {id} in database");
    }

    public async Task<TEntity> CreateAsync(TEntity entity, TOwner owner)
    {
        entity.Owner = await GetOwnerAsync(owner.Id);

        EntityEntry<TEntity> result = await _dbContext.Set<TEntity>().AddAsync(entity);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("CreateAsync created no rows");
        }

        return result.Entity;
    }

    public async Task<IEnumerable<TEntity>> ReadAllAsync(TOwner owner)
    {
        return await _dbContext.Set<TEntity>()
            .Where(e => e.Owner.Id.Equals(owner.Id))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> ReadAllAsync(TOwner owner, Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>()
            .Where(e => e.Owner.Id.Equals(owner.Id))
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TEntity> ReadAsync(TId id, TOwner owner)
    {
        TEntity? entityInDb = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entityInDb == null)
        {
            throw new EntityNotFoundException($"Could not find {typeof(TEntity)} with id {id}");
        }
        if (entityInDb.Owner.Id != owner.Id)
        {
            throw new EntityOwnershipViolationException($"{typeof(TEntity)} does not belong to {owner}");
        }

        return entityInDb;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, TOwner owner)
    {
        TEntity entityInDatabase = await ReadAsync(entity.Id, owner);
        entityInDatabase.CopyFrom(entity);

        _dbContext.Set<TEntity>().Update(entityInDatabase);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("UpdateAsync updated no rows");
        }

        return entityInDatabase;
    }

    public async Task DeleteAsync(TId id, TOwner owner)
    {
        TEntity entityInDatabase = await ReadAsync(id, owner);

        _dbContext.Set<TEntity>().Remove(entityInDatabase);

        int rowsAffected = await _dbContext.SaveChangesAsync();
        if (rowsAffected < 1)
        {
            throw new RepositorySaveChangeFailedException("DeleteAsync deleted no rows");
        }
    }
}
