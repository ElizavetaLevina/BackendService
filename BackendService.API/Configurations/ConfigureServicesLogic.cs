using BackendService.API.Utils;
using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.DAL.Repositories;
using DotNetEnv;

namespace BackendService.API.Configurations
{
    /// <summary>
    /// Конфигурация бизнес-логики, репозиториев и внешних сервисов
    /// </summary>
    public static class ConfigureServicesLogic
    {
        /// <summary>
        /// Регистрирует репозитории, логику, HttpClient и обработчики исключений
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не установлены обязательные переменные окружения: API_KEY или BASE_API_URL</exception>
        public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
        {
            // Загружаем переменные из .env файла
            Env.Load();

            // Добавляем переменные окружения в конфигурацию
            configuration.AddEnvironmentVariables();

            // Обязательные переменные окружения для внешнего API
            var apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new InvalidOperationException("API_KEY не установлен");
            var baseUrl = Environment.GetEnvironmentVariable("BASE_API_URL") ?? throw new InvalidOperationException("BASE_API_URL не установлен");

            // HttpClient для NewsApi с User-Agent
            services.AddHttpClient("NewsApi", client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "BackendService/1.0");
            });

            services.AddSingleton<INewsRepository>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient("NewsApi");
                return new NewsRepository(apiKey, baseUrl, client);
            });

            // Репозитории
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            // Бизнес-логика
            services.AddScoped<IPostLogic, PostLogic>();
            services.AddScoped<ITagLogic, TagLogic>();
            services.AddScoped<IImageLogic, ImageLogic>();

            // Глобальная обработка исключений
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}
