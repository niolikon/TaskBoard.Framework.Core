using Microsoft.AspNetCore.Identity;

namespace TaskBoard.Framework.Core.Entities;

public class PersistedOwnerEntity : IdentityUser, IOwnerEntity
{
    public override bool Equals(object? obj)
    {
        if (object.ReferenceEquals(this, obj)) 
        {
            return true;
        }
        if (obj is PersistedOwnerEntity po)
        {
            return object.Equals(this.Id, po.Id);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, UserName);
    }
}
