using MobilePay.Core.Domain.Commons;
using System.Net;
using System.Text.Json;
using static MobilePay.Core.Domain.Commons.DomainExceptions;

namespace MobilePay.Endpoints.WebApi.Extensions.Middleware;

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
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        ErrorDetails result = new(
                    400,
                    "Validation Errors",
                    context.Request.Path);

        switch (exception)
        {
            case InvalidEntityState stateEntity:
                context.Response.StatusCode = 400;
                result.SetErrors(stateEntity.Errors);
                break;
            case MerchantNameNotExistException partnerNameNotExistException:
                context.Response.StatusCode = 400;
                result.SetErrors(partnerNameNotExistException.Errors);
                break;
            case JsonFormatNotValidException jsonFormatNotValidException:
                context.Response.StatusCode = 400;
                result.SetErrors(jsonFormatNotValidException.Errors);
                break;
            default:
                result = new ErrorDetails(
                      context.Response.StatusCode,
                      exception.GetBaseException().Message,
                      context.Request.Path);
                break;
        }
        return context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        }));
    }
}
