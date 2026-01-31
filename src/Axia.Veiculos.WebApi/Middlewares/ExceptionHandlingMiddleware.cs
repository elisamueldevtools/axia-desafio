using System.Text.Json;
using Axia.Veiculos.Application.Common;
using FluentValidation;

namespace Axia.Veiculos.WebApi.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        Result result;

        switch (exception)
        {
            case ValidationException validationException:
                var errors = validationException.Errors.Select(e => e.ErrorMessage);
                result = Result.BadRequest("Erro de validação", errors);
                break;

            default:
                logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
                result = Result.InternalError();
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = result.StatusCode;

        var response = new
        {
            result.IsSuccess,
            result.Message,
            result.StatusCode,
            result.Errors
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
