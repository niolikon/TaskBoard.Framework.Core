using AutoFixture;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Identity;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Mappers;
using TaskBoard.Framework.Core.Repositories;
using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Services;

public class BaseSecuredCrudServiceTests
{
    private readonly Fixture _fixture;
    private readonly AuthenticatedUser _testUser;
    private readonly TestUser _owner;
    private readonly Mock<ISecuredCrudRepository<TestOwnedEntity, int, TestUser>> _repositoryMock;
    private readonly Mock<IMapper<TestOwnedEntity, TestInputDto, TestOutputDto>> _mapperMock;
    private readonly Mock<UserManager<TestUser>> _userManagerMock;
    private readonly BaseSecuredCrudService<TestOwnedEntity, int, TestInputDto, TestOutputDto, TestUser> _service;

    public BaseSecuredCrudServiceTests()
    {
        _fixture = new Fixture();
        _testUser = new AuthenticatedUser { Id = "1" };
        _owner = new TestUser { Id = "1", UserName = "testuser" };
        _repositoryMock = new Mock<ISecuredCrudRepository<TestOwnedEntity, int, TestUser>>();
        _mapperMock = new Mock<IMapper<TestOwnedEntity, TestInputDto, TestOutputDto>>();
        _userManagerMock = new Mock<UserManager<TestUser>>(
            (new Mock<IUserStore<TestUser>>()).Object,
            null, // IOptions<IdentityOptions>
            null, // IPasswordHasher<User>
            null, // IEnumerable<IUserValidator<User>>
            null, // IEnumerable<IPasswordValidator<User>>
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null, // IServiceProvider
            null  // ILogger<UserManager<User>>
        );
        _service = new BaseSecuredCrudService<TestOwnedEntity, int, TestInputDto, TestOutputDto, TestUser>(_repositoryMock.Object, _mapperMock.Object);

        _userManagerMock.Setup(m => m.FindByIdAsync(_testUser.Id)).ReturnsAsync(_owner);
    }

    [Fact]
    public async Task Given_ValidInput_When_Create_Then_ReturnsMappedOutputDto()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var entity = _fixture.Create<TestOwnedEntity>();
        var outputDto = _fixture.Create<TestOutputDto>();

        _mapperMock.Setup(m => m.MapToEntity(inputDto)).Returns(entity);
        _repositoryMock.Setup(r => r.CreateAsync(entity, _owner)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToOutputDto(entity)).Returns(outputDto);

        var result = await _service.CreateAsync(inputDto, _testUser);

        result.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_ExistingId_When_Read_Then_ReturnsMappedOutputDto()
    {
        var entity = _fixture.Create<TestOwnedEntity>();
        var outputDto = _fixture.Create<TestOutputDto>();

        _repositoryMock.Setup(r => r.ReadAsync(entity.Id, _owner)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToOutputDto(entity)).Returns(outputDto);

        var result = await _service.ReadAsync(entity.Id, _testUser);

        result.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task When_ReadAll_Then_ReturnsListOfMappedOutputDtos()
    {
        var entities = _fixture.CreateMany<TestOwnedEntity>(3).ToList();
        var outputDtos = entities.Select(e => _fixture.Create<TestOutputDto>()).ToList();

        _repositoryMock.Setup(r => r.ReadAllAsync(_owner)).ReturnsAsync(entities);
        for (int i = 0; i < entities.Count; i++)
        {
            _mapperMock.Setup(m => m.MapToOutputDto(entities[i])).Returns(outputDtos[i]);
        }

        var result = await _service.ReadAllAsync(_testUser);

        result.Should().BeEquivalentTo(outputDtos);
    }

    [Fact]
    public async Task Given_ValidInput_When_Update_Then_ReturnsMappedOutputDto()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var entity = _fixture.Create<TestOwnedEntity>();
        var outputDto = _fixture.Create<TestOutputDto>();

        _mapperMock.Setup(m => m.MapToEntity(inputDto)).Returns(entity);
        _repositoryMock.Setup(r => r.UpdateAsync(entity, _owner)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToOutputDto(entity)).Returns(outputDto);

        var result = await _service.UpdateAsync(entity.Id, inputDto, _testUser);

        result.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_ValidId_When_Delete_Then_DeletesEntity()
    {
        await _service.DeleteAsync(1, _testUser);
        _repositoryMock.Verify(r => r.DeleteAsync(1, _owner), Times.Once);
    }
}
