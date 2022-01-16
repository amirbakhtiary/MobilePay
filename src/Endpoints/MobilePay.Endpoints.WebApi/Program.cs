using MobilePay.Endpoints.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .CustomizeAppConfiguration()
    .CustomizeUseSerilog();

builder.Services
    .ConfigControllers()
    .AddServiceRegistry(builder.Configuration)
    .ConfigApiBehavior()
    .AddCustomizedDataStore(builder.Configuration)
    .ConfigMediatR()
    .ConfigSwagger(builder.Configuration)
    .ConfigAutoMapper();

var app = builder.Build();

await app.Services.EnsureDb();

app.UseCustomizedSwagger(builder.Configuration);
app.CustomExceptionMiddleware();

app.MapControllers();

app.Run();

public partial class Program { }