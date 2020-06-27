using Microsoft.Extensions.Configuration;
using Serilog;

namespace NServiceBus2020.ATMMachineClient
{
    public static class LoggerConfiguration
    {
        public static Serilog.Core.Logger CreateLogger(IConfigurationRoot configuration, string endpoint) =>
            new Serilog.LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("Application", endpoint)
                .CreateLogger();
    }
}
