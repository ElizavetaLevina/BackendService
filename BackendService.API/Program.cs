using AutoMapper;
using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.DAL.Mappings;
using BackendService.DAL.Models;
using BackendService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using DotNetEnv;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;

namespace BackendService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.\
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserRead", c =>
                {
                    c.RequireRealmRoles("user").RequireResourceRoles("read");
                });

                options.AddPolicy("UserEdit", c =>
                {
                    c.RequireRealmRoles("user").RequireResourceRoles("edit");
                });
            }).AddKeycloakAuthorization(builder.Configuration);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendService.API", Version = "v1" });
                options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("http://localhost:8090/realms/auth-dev/protocol/openid-connect/auth"),
                            TokenUrl = new Uri("http://localhost:8090/realms/auth-dev/protocol/openid-connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "OpenID scope" },
                                { "profile", "User profile" }
                            }
                        }
                    }
                });

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

                var xmlFail = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFail);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.EnableAnnotations();
            });

            builder.Services.AddDbContext<ApplicationDbContext>(
                option =>
                {
                    option.UseNpgsql(config.GetConnectionString(nameof(ApplicationDbContext)));
                });

            Env.Load();

            builder.Services.AddSingleton<INewsRepository, NewsRepository>(c =>
            {
                var apiKey = Environment.GetEnvironmentVariable("API_KEY");
                var baseUrl = Environment.GetEnvironmentVariable("BASE_API_URL");
                return new NewsRepository(apiKey, baseUrl);
            });

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

            builder.Services.AddScoped<IPostLogic, PostLogic>();
            builder.Services.AddScoped<ITagLogic, TagLogic>();
            builder.Services.AddScoped<IImageLogic, ImageLogic>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<AppMappingProfile>();
            }, NullLoggerFactory.Instance);

            mapperConfig.AssertConfigurationIsValid();

            builder.Services.AddSingleton(mapperConfig.CreateMapper());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendService.API v1");
                    options.OAuthClientId("backend-service");
                    options.OAuthClientSecret(builder.Configuration["Keycloak:credentials:secret"]);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
