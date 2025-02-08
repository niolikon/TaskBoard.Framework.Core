namespace TaskBoard.Framework.Core.Dtos;

public class TestOutputDto : BaseOutputDto<int>
{
    public required string Name { get; set; }
}