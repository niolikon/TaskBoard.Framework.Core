namespace TaskBoard.Framework.Core.Entities;

public abstract class BaseEntity<TId>
{
    public required TId Id { get; set; }

    public abstract void CopyFrom(BaseEntity<TId> other);
}
