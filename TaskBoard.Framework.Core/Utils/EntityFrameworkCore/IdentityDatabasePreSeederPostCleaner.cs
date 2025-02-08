using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskBoard.Framework.Core.Entities;

namespace TaskBoard.Framework.Core.Utils.EntityFrameworkCore;

public class IdentityDatabasePreSeederPostCleaner<TDbContext, TOwner, TEntityId> : DatabasePreSeederPostCleaner<TDbContext>
    where TDbContext : DbContext
    where TOwner : PersistedOwnerEntity
{
    protected readonly UserManager<TOwner> _userManager;

    public IdentityDatabasePreSeederPostCleaner(TDbContext context, UserManager<TOwner> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public override void PopulateDatabase(object[] data)
    {
        _context.Database.EnsureCreated();
        DisableForeignKeys();

        var ownerships = from owner in data
                         where owner is TOwner
                         select new {
                            Owner = owner as TOwner,
                            Belongings = (from obj in data
                                        where (obj is BaseOwnedEntity<TEntityId, TOwner> owned) &&
                                                (owned.Owner == owner)
                                        select obj as BaseOwnedEntity<TEntityId, TOwner>).ToList()
                         };

        foreach(var entry in ownerships)
        {
            IdentityResult result = _userManager.CreateAsync(entry.Owner, entry.Owner.PasswordHash).GetAwaiter().GetResult();
            if (! result.Succeeded)
            {
                StringBuilder errorStringBuilder = new StringBuilder();
                foreach(IdentityError error in result.Errors)
                {
                    errorStringBuilder.Append($"{error.Code}: {error.Description},");
                }
                errorStringBuilder[^1] = ' ';
                ;
                throw new InvalidDataException(errorStringBuilder.ToString().TrimEnd());
            }

            foreach(var belonging in entry.Belongings)
            {
                BaseOwnedEntity<TEntityId, TOwner> entity = belonging;
                entity.Owner = entry.Owner;
                _context.Add(belonging);
            }
        }

        var ownershipEntities = ownerships.SelectMany(entry => {
            List<object> result = new List<object>();
            result.Add(entry.Owner);
            result.AddRange(entry.Belongings);
            return result;
        }).Distinct();

        var otherEntities = data.Where(entity => !ownershipEntities.Contains(entity));
        foreach (var entry in otherEntities)
        {
            _context.Add(entry);
        }

        _context.SaveChanges();

        EnableForeignKeys();
    }
}
