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
