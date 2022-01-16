using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;
using MobilePay.Core.Domain.Commons;
using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Core.Domain.TransactionAggregate.Dto;
using MobilePay.Infrastructures.Data.Commons;
using System.Reflection;

namespace MobilePay.Endpoints.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new MerchantList(configuration.GetSection("Merchants").Get<List<Merchant>>()));
        services.AddSingleton(configuration.GetSection("FeeSetting").Get<FeeSetting>());


        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Scan(s => s.FromAssemblies(
            Assembly.Load(typeof(MobilePayDbContext).GetTypeInfo().Assembly.GetName().Name),
            Assembly.Load(typeof(CreateTransactionRequestCommandHandler).GetTypeInfo().Assembly.GetName().Name),
            Assembly.Load(typeof(IUnitOfWork).GetTypeInfo().Assembly.GetName().Name),
            Assembly.Load(typeof(UnitOfWork).GetTypeInfo().Assembly.GetName().Name))
        .AddClasses(c => c.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Lookup")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection ConfigMediatR(this IServiceCollection services)
    {
        services.AddMediatR(
            Assembly.Load(typeof(CreateTransactionRequestCommandHandler).GetTypeInfo().Assembly.GetName().Name),
            Assembly.Load(typeof(MobilePayDbContext).GetTypeInfo().Assembly.GetName().Name));
        return services;
    }

    public static IServiceCollection ConfigAutoMapper(this IServiceCollection services)
    {
        services.AddTransient(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new Mapper.DataProfile(provider.GetService<MerchantList>(),
                provider.GetService<FeeSetting>()));
        }).CreateMapper());
        return services;
    }

    public static IServiceCollection ConfigControllers(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    public static IServiceCollection ConfigApiBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(new ErrorDetails(problemDetails.Status, problemDetails.Title, problemDetails.Instance,
                    context.ModelState.Values.SelectMany(x => x.Errors)
                        .Select(x => new Error(x.ErrorMessage))
                    ))
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });
        return services;
    }

    public static IServiceCollection ConfigSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        if (configuration.GetValue<bool>("SwaggerEnabled"))
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mobile Pay HTTP API",
                    Version = "v1.0",
                    Description = "The Mobile Pay Service HTTP API",
                });

            });
        return services;
    }

    public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MobilePayDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),
            b => b.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name)));

        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
}