using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Exceptions.Persistence;
using TaskBoard.Framework.Core.Exceptions.Rest;
using TaskBoard.Framework.Core.Mappers;
using TaskBoard.Framework.Core.Repositories;
using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Services;

public class BaseSecuredCrudService<TEntity, TId, TInputDto, TOutputDto, TOwner> : ISecuredCrudService<TId, TInputDto, TOutputDto>
    where TEntity : BaseOwnedEntity<TId, TOwner>
    where TInputDto : class
    where TOutputDto : BaseOutputDto<TId>
    where TOwner : IOwnerEntity, new()
{
    protected ISecuredCrudRepository<TEntity, TId, TOwner> _repository;
    protected IMapper<TEntity, TInputDto, TOutputDto> _mapper;

    public BaseSecuredCrudService(
        ISecuredCrudRepository<TEntity, TId, TOwner> repository,
        IMapper<TEntity, TInputDto, TOutputDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    protected static TOwner GetOwner(AuthenticatedUser authenticatedUser)
    {
        return AuthenticatedUserMapper.MapToOwnerEntity<TOwner>(authenticatedUser);
    }

    public async Task<TOutputDto> CreateAsync(TInputDto dto, AuthenticatedUser user)
    {
        TOwner owner = GetOwner(user);
        TEntity entity = _mapper.MapToEntity(dto);
        try
        {
            TEntity entityInDb = await _repository.CreateAsync(entity, owner);
            return _mapper.MapToOutputDto(entityInDb);
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not create entity");
        }
    }

    public async Task<IEnumerable<TOutputDto>> ReadAllAsync(AuthenticatedUser user)
    {
        TOwner owner = GetOwner(user);
        IEnumerable<TEntity> entities = await _repository.ReadAllAsync(owner);
        return entities.Select(_mapper.MapToOutputDto).ToList();
    }

    public async Task<TOutputDto> ReadAsync(TId id, AuthenticatedUser user)
    {
        TOwner owner = GetOwner(user);
        try
        {
            TEntity entity = await _repository.ReadAsync(id, owner);
            return _mapper.MapToOutputDto(entity);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (EntityOwnershipViolationException)
        {
            throw new UnauthorizedRestException("User not authorized to read entities under others' ownership");
        }
    }

    public async Task<TOutputDto> UpdateAsync(TId id, TInputDto dto, AuthenticatedUser user)
    {
        TOwner owner = GetOwner(user);
        TEntity entityWithUpdatedData = _mapper.MapToEntity(dto);
        entityWithUpdatedData.Id = id;

        try
        {
            TEntity entityUpdated = await _repository.UpdateAsync(entityWithUpdatedData, owner);
            return _mapper.MapToOutputDto(entityUpdated);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (EntityOwnershipViolationException)
        {
            throw new UnauthorizedRestException("User not authorized to update entities under others' ownership");
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not update entity");
        }
    }

    public async Task DeleteAsync(TId id, AuthenticatedUser user)
    {
        TOwner owner = GetOwner(user);

        try
        {
            await _repository.DeleteAsync(id, owner);
        }
        catch (EntityNotFoundException ex)
        {
            throw new NotFoundRestException(ex.Message);
        }
        catch (EntityOwnershipViolationException)
        {
            throw new UnauthorizedRestException("User not authorized to delete entities under others' ownership");
        }
        catch (RepositorySaveChangeFailedException)
        {
            throw new ConflictRestException("Could not delete entity");
        }
    }
}
