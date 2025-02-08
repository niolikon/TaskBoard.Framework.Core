using TaskBoard.Framework.Core.Dtos;

namespace TaskBoard.Framework.Core.Services;

public interface ICrudService<TId, TInputDto, TOutputDto>
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
{
    Task<TOutputDto> CreateAsync(TInputDto dto);
    Task<IEnumerable<TOutputDto>> ReadAllAsync();
    Task<TOutputDto> ReadAsync(TId id);
    Task<TOutputDto> UpdateAsync(TId id, TInputDto dto);
    Task DeleteAsync(TId id);
}
