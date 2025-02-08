namespace TaskBoard.Framework.Core.Mappers;

public interface IMapper<TEntity, TInputDto, TOutputDto>
{
    TEntity MapToEntity(TInputDto dto);

    TOutputDto MapToOutputDto(TEntity entity);
}
