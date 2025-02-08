using Microsoft.EntityFrameworkCore;

namespace TaskBoard.Framework.Core.Entities;

[Owned]
public class ServedOwnerEntity : IOwnerEntity
{
    public string Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj is ServedOwnerEntity so)
        {
            return object.Equals(this.Id, so.Id);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}
