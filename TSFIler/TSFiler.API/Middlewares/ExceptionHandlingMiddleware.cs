using Microsoft.AspNetCore.Mvc;
using TSFiler.Common.Exceptions;

namespace TSFiler.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (IncorrectDataException exception)
        {
            await HandleExceptionAsync(context, exception, StatusCodes.Status400BadRequest);
        }
        catch (DivideByZeroException exception)
        {
            await HandleExceptionAsync(context, exception, StatusCodes.Status400BadRequest);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, StatusCodes.Status500InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = exception.Message
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
