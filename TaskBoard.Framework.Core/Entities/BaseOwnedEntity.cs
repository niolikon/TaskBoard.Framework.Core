using TaskBoard.Framework.Core.Entities;

namespace TaskBoard.Framework.Core.Entities;

public abstract class BaseOwnedEntity<TId, TOwner>
    where TOwner : IOwnerEntity
{
    public required TId Id { get; set; }

    public required TOwner Owner { get; set; }

    public abstract void CopyFrom(BaseOwnedEntity<TId, TOwner> other);
}
