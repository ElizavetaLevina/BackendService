using BackendService.BLL.Consumers;
using MassTransit;

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

                    //cfg.ReceiveEndpoint("PostSubmittedForModeration", e =>
                    //{
                    //    e.Bind("PostSubmittedForModeration");
                    //    e.ConfigureConsumer<PostModeratedEventConsumer>(context);
                    //});
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
