using Microsoft.EntityFrameworkCore;
using MobilePay.Endpoints.WebApi.Extensions.Middleware;
using MobilePay.Infrastructures.Data.Commons;

namespace MobilePay.Endpoints.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void CustomExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();

    public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MobilePay V1");
            });

        return app;
    }

    public static async Task EnsureDb(this IServiceProvider service)
    {
        using var db = service.CreateScope().ServiceProvider.GetRequiredService<MobilePayDbContext>();
        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync();
        }

    }
}
