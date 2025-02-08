namespace TaskBoard.Framework.Core.Entities;

public class TestOwnedEntity : BaseOwnedEntity<int, TestUser>
{
    public required string Name { get; set; }

    public override void CopyFrom(BaseOwnedEntity<int, TestUser> other)
    {
        if (other is TestOwnedEntity te)
        {
            if (!string.IsNullOrEmpty(te.Name))
            {
                this.Name = te.Name;
            }
        }
    }
}
