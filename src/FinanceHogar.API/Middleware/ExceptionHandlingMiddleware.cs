using System.Net;
using System.Text.Json;

namespace FinanceHogar.API.Middleware;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no manejado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, mensaje) = ex switch
        {
            KeyNotFoundException        => (HttpStatusCode.NotFound,              ex.Message),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden,             ex.Message),
            ArgumentException           => (HttpStatusCode.BadRequest,            ex.Message),
            InvalidOperationException   => (HttpStatusCode.Conflict,              ex.Message),
            _                           => (HttpStatusCode.InternalServerError,   "Ocurrió un error interno. Intente más tarde.")
        };

        context.Response.StatusCode  = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error     = mensaje,
            status    = (int)statusCode,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
