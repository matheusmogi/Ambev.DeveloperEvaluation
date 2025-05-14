using Ambev.DeveloperEvaluation.Messages;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class MessageModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<SaleCreatedEventConsumer>();
            x.AddConsumer<SaleUpdatedEventConsumer>();
            x.AddConsumer<SaleDeletedEventConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
                cfg.Host(settings.HostName, h =>
                {
                    h.Username(settings.UserName);
                    h.Password(settings.Password);
                });
                cfg.ReceiveEndpoint("sale.created", e =>
                {
                    e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));
                    e.ConfigureConsumer<SaleCreatedEventConsumer>(context);
                    e.BindDeadLetterQueue("sale.created.dlq");
                    e.DiscardSkippedMessages();
                });
                cfg.ReceiveEndpoint("sale.updated", e =>
                {
                    e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));
                    e.ConfigureConsumer<SaleUpdatedEventConsumer>(context);
                    e.BindDeadLetterQueue("sale.updated.dlq");
                    e.DiscardSkippedMessages();
                });
                cfg.ReceiveEndpoint("sale.deleted", e =>
                {
                    e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));
                    e.ConfigureConsumer<SaleDeletedEventConsumer>(context);
                    e.BindDeadLetterQueue("sale.deleted.dlq");
                    e.DiscardSkippedMessages();
                });
            });
        });
    }
}