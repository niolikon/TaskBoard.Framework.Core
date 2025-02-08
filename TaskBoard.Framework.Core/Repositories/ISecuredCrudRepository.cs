using System.Linq.Expressions;
using TaskBoard.Framework.Core.Entities;

namespace TaskBoard.Framework.Core.Repositories;

public interface ISecuredCrudRepository<TEntity, TId, TOwner> 
    where TEntity : BaseOwnedEntity<TId, TOwner>
    where TOwner : IOwnerEntity, new()
{
    Task<TEntity> CreateAsync(TEntity entity, TOwner owner);
    Task<IEnumerable<TEntity>> ReadAllAsync(TOwner owner);
    Task<IEnumerable<TEntity>> ReadAllAsync(TOwner owner, Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> ReadAsync(TId id, TOwner owner);
    Task<TEntity> UpdateAsync(TEntity entity, TOwner owner);
    Task DeleteAsync(TId id, TOwner owner);
}
