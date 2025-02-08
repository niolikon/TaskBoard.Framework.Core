namespace TaskBoard.Framework.Core.Dtos;

public class BaseOutputDto<TId>
{
    public required TId Id { get; set; }
}
