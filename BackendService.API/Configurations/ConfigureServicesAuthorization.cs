using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BackendService.API.Configurations
{
    /// <summary>
    /// Конфигурация авторизации и аутентификации с Keycloak
    /// </summary>
    public static class ConfigureServicesAuthorization
    {
        /// <summary>
        /// Регистрирует JWT аутентификацию и политики авторизации
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <param name="isInDocker">Флаг запуска в контейнере</param>
        public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration, bool isInDocker)
        {
            // Выбираем URL Keycloak в зависимости от окружения
            var keycloakAuthUrl = isInDocker ? configuration["Keycloak:AuthorityDocker"] : configuration["Keycloak:AuthorityLocal"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = keycloakAuthUrl;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Keycloak:AuthorityLocal"],
                    ValidateAudience = true,
                    ValidAudiences = new[]
                    {
                        "backend-service",
                        "account"
                    }
                };
            });

            // Политики авторизации на основе ролей и ресурсов Keycloak
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserRead", c =>
                {
                    c.RequireRealmRoles("user").RequireResourceRoles("read");
                });

                options.AddPolicy("UserEdit", c =>
                {
                    c.RequireRealmRoles("user").RequireResourceRoles("edit");
                });

                options.AddPolicy("Admin", c =>
                {
                    c.RequireRealmRoles("admin");
                });
            }).AddKeycloakAuthorization(configuration);
        }
    }
}
