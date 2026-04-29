using AutoMapper;
using BackendService.API.Utils;
using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.DAL.Mappings;
using BackendService.DAL.Models;
using BackendService.DAL.Repositories;
using DotNetEnv;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.Net.Models.Clients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using System.Reflection;

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

            var isInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            if (isInDocker)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File("/app/logs/log-.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                    .MinimumLevel.Override("Microsoft.Extensions", LogEventLevel.Error)
                   // .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = isInDocker == true ? "http://host.docker.internal:8090/realms/auth-dev" : "http://localhost:8090/realms/auth-dev";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidAudiences = new[]
                    {
                        "backend-service",
                        "account"
                    }
                };
            });

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

                options.AddPolicy("Admin", c =>
                {
                    c.RequireRealmRoles("admin");
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

            var connectionStringName = isInDocker == true ? nameof(ApplicationDbContext) : $"{nameof(ApplicationDbContext)}_Local";
            builder.Services.AddDbContext<ApplicationDbContext>(
                option =>
                {
                    option.UseNpgsql(config.GetConnectionString(connectionStringName));
                });

            Env.Load();

            builder.Configuration.AddEnvironmentVariables();

            var apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new InvalidOperationException("API_KEY не установлен");
            var baseUrl = Environment.GetEnvironmentVariable("BASE_API_URL") ?? throw new InvalidOperationException("BASE_API_URL не установлен");

            builder.Services.AddHttpClient("NewsApi", client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "BackendService/1.0");
            });

            builder.Services.AddSingleton<INewsRepository>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient("NewsApi");
                return new NewsRepository(apiKey, baseUrl, client);
            });

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
                    options.OAuthClientId(builder.Configuration["Keycloak:resource"]);
                    options.OAuthClientSecret(builder.Configuration["Keycloak:credentials:secret"]);
                });
            }

            app.UseExceptionHandler();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
