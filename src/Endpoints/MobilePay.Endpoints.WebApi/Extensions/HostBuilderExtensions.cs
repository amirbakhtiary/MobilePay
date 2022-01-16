using Serilog;

namespace MobilePay.Endpoints.WebApi.Extensions;

public static class HostBuilderExtensions
{
    public static ConfigureHostBuilder CustomizeAppConfiguration(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("merchants.json", optional: false, reloadOnChange: false);
        });
        return host;
    }
    public static ConfigureHostBuilder CustomizeUseSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "MobilePay")
                .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
        });
        return host;
    }
}
