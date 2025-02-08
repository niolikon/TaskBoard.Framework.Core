namespace TaskBoard.Framework.Core.Entities;

public class TestEntity : BaseEntity<int>
{
    public required string Name { get; set; }

    public override void CopyFrom(BaseEntity<int> other)
    {
        if (other is TestEntity te) 
        {
            if (!string.IsNullOrEmpty(te.Name))
            {
                this.Name = te.Name;
            }
        }
    }
}
