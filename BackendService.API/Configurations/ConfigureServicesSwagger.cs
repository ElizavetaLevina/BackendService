using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BackendService.API.Configurations
{
    /// <summary>
    /// Конфигурация Swagger
    /// </summary>
    public static class ConfigureServicesSwagger
    {
        /// <summary>
        /// Регистрирует SwaggerGen с OAuth2 авторизацией через Keycloak
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация приложения</param>
        public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
        {
            var authority = configuration["Keycloak:AuthorityLocal"];

            var authUrl = $"{authority}/protocol/openid-connect/auth";
            var tokenUrl = $"{authority}/protocol/openid-connect/token";

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendService.API", Version = "v1" });
                // Настройка OAuth2 авторизации для Swagger UI
                options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authUrl),
                            TokenUrl = new Uri(tokenUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "OpenID scope" },
                                { "profile", "User profile" }
                            }
                        }
                    }
                });

                // Добавляем кнопку Authorize в Swagger
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Keycloak"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Подключаем XML комментарии для документации
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.EnableAnnotations();
            });
        }

        /// <summary>
        /// Настройка Swagger UI
        /// </summary>
        /// <param name="app">WebApplication для настройки Swagger и SwaggerUI</param>
        /// <param name="configuration">Конфигурация приложения</param>
        public static void Configure(WebApplication app, ConfigurationManager configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendService.API v1");
                options.OAuthClientId(configuration["Keycloak:resource"]);
                options.OAuthClientSecret(configuration["Keycloak:credentials:secret"]);
            });
        }
    }
}
