using System.Net;
using System.Text.Json;
using ShAbedi.PayaSystem.Application.Exceptions;
using ShAbedi.PayaSystem.Domain.Exceptions;

namespace ShAbedi.PayaSystem.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;
        string errorCode;
        string message = exception.Message;

        switch (exception)
        {
            case DomainException dom:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = dom.ErrorCode;
                break;
            case BusinessException be:
                statusCode = HttpStatusCode.BadRequest;
                errorCode =be.ErrorCode;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = "INTERNAL_ERROR";
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new
        {
            message,
            code = errorCode
        });

        return context.Response.WriteAsync(result);
    }
}