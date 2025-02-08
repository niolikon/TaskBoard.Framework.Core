using Microsoft.AspNetCore.Mvc;
using AutoFixture;
using FluentAssertions;
using Moq;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Security.Authentication;
using TaskBoard.Framework.Core.Services;

namespace TaskBoard.Framework.Core.Controllers;

public class BaseSecuredCrudControllerTests
{
    private readonly Fixture _fixture;
    private readonly AuthenticatedUser _testUser;
    private readonly Mock<ISecuredCrudService<int, TestInputDto, TestOutputDto>> _serviceMock;
    private readonly Mock<IAuthenticatedUserService> _userServiceMock;
    private readonly BaseSecuredCrudController<int, TestInputDto, TestOutputDto> _controller;

    public BaseSecuredCrudControllerTests()
    {
        _fixture = new Fixture();
        _testUser = new AuthenticatedUser { Id = "0123-4567-89ab-cdef" };
        _serviceMock = new Mock<ISecuredCrudService<int, TestInputDto, TestOutputDto>>();
        _userServiceMock = new Mock<IAuthenticatedUserService>();
        _controller = new BaseSecuredCrudController<int, TestInputDto, TestOutputDto>(_serviceMock.Object, _userServiceMock.Object);
        _userServiceMock.Setup(f => f.User).Returns(_testUser);
    }

    [Fact]
    public async Task Given_ValidInput_When_Create_Then_CreatedAtActionIsReturned()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.CreateAsync(inputDto, _testUser)).ReturnsAsync(outputDto);

        ActionResult<TestOutputDto> result = await _controller.Create(inputDto);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
        CreatedAtActionResult actionResult = (CreatedAtActionResult)result.Result;
        actionResult.Value.Should().BeEquivalentTo(outputDto);
        actionResult.ActionName.Should().BeEquivalentTo("Read");
    }

    [Fact]
    public async Task Given_ValidInput_When_Create_Then_RequestIsRelayed()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.CreateAsync(inputDto, _testUser)).ReturnsAsync(outputDto);

        await _controller.Create(inputDto);

        _serviceMock.Verify(s => s.CreateAsync(It.IsAny<TestInputDto>(), It.IsAny<AuthenticatedUser>()), Times.Once);
    }

    [Fact]
    public async Task Given_ExistingId_When_Read_Then_ReturnsOkWithDto()
    {
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.ReadAsync(1, _testUser)).ReturnsAsync(outputDto);

        ActionResult<TestOutputDto> result = await _controller.ReadAsync(1);

        result.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult actionResult = (OkObjectResult)result.Result;
        actionResult.Value.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_ExistingId_When_Read_Then_RequestIsRelayed()
    {
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.ReadAsync(1, _testUser)).ReturnsAsync(outputDto);

        await _controller.ReadAsync(1);

        _serviceMock.Verify(s => s.ReadAsync(It.IsAny<int>(), It.IsAny<AuthenticatedUser>()), Times.Once);
    }

    [Fact]
    public async Task When_ReadAll_Then_ReturnsOkWithList()
    {
        var outputDtos = _fixture.CreateMany<TestOutputDto>(3);
        _serviceMock.Setup(s => s.ReadAllAsync(_testUser)).ReturnsAsync(outputDtos);

        ActionResult<IEnumerable<TestOutputDto>> result = await _controller.ReadAllAsync();

        result.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult actionResult = (OkObjectResult)result.Result;
        actionResult.Value.Should().BeEquivalentTo(outputDtos);
    }

    [Fact]
    public async Task When_ReadAll_Then_RequestIsRelayed()
    {
        var outputDtos = _fixture.CreateMany<TestOutputDto>(3);
        _serviceMock.Setup(s => s.ReadAllAsync(_testUser)).ReturnsAsync(outputDtos);

        await _controller.ReadAllAsync();

        _serviceMock.Verify(s => s.ReadAllAsync(It.IsAny<AuthenticatedUser>()), Times.Once);
    }

    [Fact]
    public async Task Given_ValidInput_When_Update_Then_ReturnsOkWithDto()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.UpdateAsync(1, inputDto, _testUser)).ReturnsAsync(outputDto);

        ActionResult<TestOutputDto> result = await _controller.UpdateAsync(1, inputDto);

        result.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult actionResult = (OkObjectResult)result.Result;
        actionResult.Value.Should().BeEquivalentTo(outputDto);
    }


    [Fact]
    public async Task Given_ValidInput_When_Update_Then_RequestIsRelayed()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.UpdateAsync(1, inputDto, _testUser)).ReturnsAsync(outputDto);

        await _controller.UpdateAsync(1, inputDto);

        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<TestInputDto>(), It.IsAny<AuthenticatedUser>()), Times.Once);
    }

    [Fact]
    public async Task Given_ExistingId_When_Delete_Then_ReturnsNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1, _testUser)).Returns(Task.CompletedTask);

        ActionResult result = await _controller.DeleteAsync(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Given_ExistingId_When_Delete_Then_RequestIsRelayed()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1, _testUser)).Returns(Task.CompletedTask);

        await _controller.DeleteAsync(1);

        _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<AuthenticatedUser>()), Times.Once);
    }
}
