using BackendService.BLL.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BackendService.API.Configurations
{
    public static class ConfigureServicesRabbitMQ
    {
        public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration, bool isInDocker)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<PostModeratedEventConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    var host = isInDocker ? configuration["RabbitMQ:HostDocker"] : configuration["RabbitMQ:HostLocal"];
                    cfg.Host($"rabbitmq://{host}:{configuration["RabbitMQ:Port"]}", x =>
                    {
                        x.Username(configuration["RabbitMQ:UserName"]);
                        x.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.UseMessageRetry(x =>
                    {
                        x.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(10));
						x.Handle<NpgsqlException>();
						x.Handle<TimeoutException>();
						x.Handle<DbUpdateException>();
					});

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
