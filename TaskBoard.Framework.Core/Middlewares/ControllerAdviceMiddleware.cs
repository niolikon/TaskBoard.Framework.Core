using Microsoft.AspNetCore.Http;
using System.Net;
using TaskBoard.Framework.Core.Dtos;
using TaskBoard.Framework.Core.Exceptions;

namespace TaskBoard.Framework.Core.Middlewares;

public class ControllerAdviceMiddleware
{
    private readonly RequestDelegate _next;

    public ControllerAdviceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseRestException re)
        {
            ErrorDto error = new (re);
            await WriteErrorResponseAsync(context, error);
        }
        catch (Exception e)
        {
            ErrorDto error = new ErrorDto("An unexpected error occurred", (int)HttpStatusCode.InternalServerError);
            await WriteErrorResponseAsync(context, error);
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, ErrorDto errorDto)
    {
        context.Response.StatusCode = errorDto.Code;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(errorDto);
    }
}
