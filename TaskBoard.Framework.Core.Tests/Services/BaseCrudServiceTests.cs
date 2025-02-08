using AutoFixture;
using FluentAssertions;
using Moq;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Exceptions.Persistence;
using TaskBoard.Framework.Core.Exceptions.Rest;
using TaskBoard.Framework.Core.Mappers;
using TaskBoard.Framework.Core.Repositories;

namespace TaskBoard.Framework.Core.Services;

public class BaseCrudServiceTests
{
    private readonly Mock<ICrudRepository<TestEntity, int>> _repositoryMock;
    private readonly Mock<IMapper<TestEntity, TestInputDto, TestOutputDto>> _mapperMock;
    private readonly BaseCrudService<TestEntity, int, TestInputDto, TestOutputDto> _service;
    private readonly Fixture _fixture;

    public BaseCrudServiceTests()
    {
        _repositoryMock = new Mock<ICrudRepository<TestEntity, int>>();
        _mapperMock = new Mock<IMapper<TestEntity, TestInputDto, TestOutputDto>>();
        _service = new BaseCrudService<TestEntity, int, TestInputDto, TestOutputDto>(_repositoryMock.Object, _mapperMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Given_ValidInput_When_Create_Then_ReturnsMappedOutputDto()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var entity = _fixture.Create<TestEntity>();
        var outputDto = _fixture.Create<TestOutputDto>();

        _mapperMock.Setup(m => m.MapToEntity(inputDto)).Returns(entity);
        _repositoryMock.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToOutputDto(entity)).Returns(outputDto);

        var result = await _service.CreateAsync(inputDto);

        result.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_RepositorySaveFails_When_Create_Then_ThrowsConflictRestException()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var entity = _fixture.Create<TestEntity>();

        _mapperMock.Setup(m => m.MapToEntity(inputDto)).Returns(entity);
        _repositoryMock.Setup(r => r.CreateAsync(entity)).ThrowsAsync(new RepositorySaveChangeFailedException("Save change failed"));

        Func<Task> act = async () => await _service.CreateAsync(inputDto);
        await act.Should().ThrowAsync<ConflictRestException>().WithMessage("Could not create entity");
    }

    [Fact]
    public async Task Given_ExistingId_When_Read_Then_ReturnsMappedOutputDto()
    {
        var entity = _fixture.Create<TestEntity>();
        var outputDto = _fixture.Create<TestOutputDto>();

        _repositoryMock.Setup(r => r.ReadAsync(entity.Id)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToOutputDto(entity)).Returns(outputDto);

        var result = await _service.ReadAsync(entity.Id);

        result.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_NonExistingId_When_Read_Then_ThrowsNotFoundRestException()
    {
        _repositoryMock.Setup(r => r.ReadAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException("Entity not found"));

        Func<Task> act = async () => await _service.ReadAsync(1);
        await act.Should().ThrowAsync<NotFoundRestException>().WithMessage("Entity not found");
    }

    [Fact]
    public async Task Given_ValidInput_When_Update_Then_ReturnsMappedOutputDto()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var entity = _fixture.Create<TestEntity>();
        var outputDto = _fixture.Create<TestOutputDto>();

        _mapperMock.Setup(m => m.MapToEntity(inputDto)).Returns(entity);
        _repositoryMock.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToOutputDto(entity)).Returns(outputDto);

        var result = await _service.UpdateAsync(entity.Id, inputDto);

        result.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_ValidId_When_Delete_Then_DeletesEntity()
    {
        await _service.DeleteAsync(1);
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistingId_When_Delete_Then_ThrowsNotFoundRestException()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException("Entity not found"));

        Func<Task> act = async () => await _service.DeleteAsync(1);
        await act.Should().ThrowAsync<NotFoundRestException>().WithMessage("Entity not found");
    }
}
