using Serilog;
using Serilog.Events;

namespace BackendService.API.Configurations
{
    /// <summary>
    /// Конфигурация логирования с использованием Serilog
    /// </summary>
    public static class ConfigureServicesLogger
    {
        /// <summary>
        /// Настраивает Serilog с разными конфигурациями для Docker и локального окружения
        /// </summary>
        /// <param name="builder">WebApplicationBuilder для настройки Host с Serilog</param>
        /// <param name="isInDocker">Флаг запуска в контейнере</param>
        public static void ConfigureServices(WebApplicationBuilder builder, bool isInDocker)
        {
            if (isInDocker)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File("/app/logs/log-.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                    .MinimumLevel.Override("Microsoft.Extensions", LogEventLevel.Error)
                    .MinimumLevel.Override("System", LogEventLevel.Error)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
                    .Filter.ByIncludingOnly(logEvent =>
                    {
                        if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContext))
                        {
                            var context = sourceContext.ToString();
                            if (context.Contains("MassTransit"))
                            {
                                var message = logEvent.RenderMessage();
                                return message.Contains("SEND") || message.Contains("RECEIVE") || message.Contains("SKIP") || logEvent.Level >= LogEventLevel.Error;
                            }
                        }
                        return true;
                    })
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();
            }

            builder.Host.UseSerilog();
        }
    }
}
