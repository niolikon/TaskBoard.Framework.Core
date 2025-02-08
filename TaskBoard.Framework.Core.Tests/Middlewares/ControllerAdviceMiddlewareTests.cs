using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using FluentAssertions;
using TaskBoard.Framework.Core.Exceptions;

namespace TaskBoard.Framework.Core.Middlewares;

public class ControllerAdviceMiddlewareTests
{
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly ControllerAdviceMiddleware _middleware;
    private readonly DefaultHttpContext _context;

    public ControllerAdviceMiddlewareTests()
    {
        _nextMock = new Mock<RequestDelegate>();
        _middleware = new ControllerAdviceMiddleware(_nextMock.Object);
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task Given_NoException_When_InvokeAsync_Then_CallsNext()
    {
        await _middleware.InvokeAsync(_context);

        _nextMock.Verify(next => next(_context), Times.Once);
    }

    [Fact]
    public async Task Given_BaseRestException_When_InvokeAsync_Then_ReturnsRestError()
    {
        var exception = new BaseRestException("Test error", HttpStatusCode.BadRequest);
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        await _middleware.InvokeAsync(_context);
        string responseBodyAsText = await FetchTextFromStream(_context.Response.Body, Encoding.UTF8);

        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        _context.Response.ContentType.Should().Contain("application/json");
        responseBodyAsText.Should().Contain("Test error");
    }

    [Fact]
    public async Task Given_GenericException_When_InvokeAsync_Then_ReturnsInternalServerError()
    {
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(new Exception("Unexpected error"));

        await _middleware.InvokeAsync(_context);
        string responseBodyAsText = await FetchTextFromStream(_context.Response.Body, Encoding.UTF8);

        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        _context.Response.ContentType.Should().Contain("application/json");
        responseBodyAsText.Should().Contain("An unexpected error occurred");
    }

    private static async Task<string> FetchTextFromStream(Stream input, Encoding encoding)
    {
        input.Seek(0, SeekOrigin.Begin);
        return await new StreamReader(input, encoding).ReadToEndAsync();
    }
}
