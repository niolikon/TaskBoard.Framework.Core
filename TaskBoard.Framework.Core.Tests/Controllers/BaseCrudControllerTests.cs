using Microsoft.AspNetCore.Mvc;
using AutoFixture;
using FluentAssertions;
using Moq;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Services;

namespace TaskBoard.Framework.Core.Controllers;

public class BaseCrudControllerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ICrudService<int, TestInputDto, TestOutputDto>> _serviceMock;
    private readonly BaseCrudController<int, TestInputDto, TestOutputDto> _controller;

    public BaseCrudControllerTests()
    {
        _fixture = new Fixture();
        _serviceMock = new Mock<ICrudService<int, TestInputDto, TestOutputDto>>();
        _controller = new BaseCrudController<int, TestInputDto, TestOutputDto>(_serviceMock.Object);
    }

    [Fact]
    public async Task Given_ValidInput_When_Create_Then_CreatedAtActionIsReturned()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.CreateAsync(inputDto)).ReturnsAsync(outputDto);

        ActionResult<TestOutputDto> result = await _controller.CreateAsync(inputDto);

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
        _serviceMock.Setup(s => s.CreateAsync(inputDto)).ReturnsAsync(outputDto);

        await _controller.CreateAsync(inputDto);

        _serviceMock.Verify(s => s.CreateAsync(It.IsAny<TestInputDto>()), Times.Once());
    }

    [Fact]
    public async Task Given_ExistingId_When_Read_Then_ReturnsOkWithDto()
    {
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.ReadAsync(1)).ReturnsAsync(outputDto);

        ActionResult<TestOutputDto> result = await _controller.ReadAsync(1);

        result.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult actionResult = (OkObjectResult)result.Result;
        actionResult.Value.Should().BeEquivalentTo(outputDto);
    }

    [Fact]
    public async Task Given_ExistingId_When_Read_Then_RequestIsRelayed()
    {
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.ReadAsync(1)).ReturnsAsync(outputDto);

        await _controller.ReadAsync(1);

        _serviceMock.Verify(s => s.ReadAsync(It.IsAny<int>()), Times.Once());
    }

    [Fact]
    public async Task When_ReadAll_Then_ReturnsOkWithList()
    {
        var outputDtos = _fixture.CreateMany<TestOutputDto>(3);
        _serviceMock.Setup(s => s.ReadAllAsync()).ReturnsAsync(outputDtos);

        ActionResult<IEnumerable<TestOutputDto>> result = await _controller.ReadAllAsync();

        result.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult actionResult = (OkObjectResult)result.Result;
        actionResult.Value.Should().BeEquivalentTo(outputDtos);
    }

    [Fact]
    public async Task When_ReadAll_Then_RequestIsRelayed()
    {
        var outputDtos = _fixture.CreateMany<TestOutputDto>(3);
        _serviceMock.Setup(s => s.ReadAllAsync()).ReturnsAsync(outputDtos);

        await _controller.ReadAllAsync();

        _serviceMock.Verify(s => s.ReadAllAsync(), Times.Once());
    }

    [Fact]
    public async Task Given_ValidInput_When_Update_Then_ReturnsOkWithDto()
    {
        var inputDto = _fixture.Create<TestInputDto>();
        var outputDto = _fixture.Build<TestOutputDto>().With(x => x.Id, 1).Create();
        _serviceMock.Setup(s => s.UpdateAsync(1, inputDto)).ReturnsAsync(outputDto);

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
        _serviceMock.Setup(s => s.UpdateAsync(1, inputDto)).ReturnsAsync(outputDto);

        await _controller.UpdateAsync(1, inputDto);

        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<TestInputDto>()), Times.Once);
    }

    [Fact]
    public async Task Given_ExistingId_When_Delete_Then_ReturnsNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        ActionResult result = await _controller.DeleteAsync(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Given_ExistingId_When_Delete_Then_RequestIsRelayed()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _controller.DeleteAsync(1);

        _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Once);
    }
}
