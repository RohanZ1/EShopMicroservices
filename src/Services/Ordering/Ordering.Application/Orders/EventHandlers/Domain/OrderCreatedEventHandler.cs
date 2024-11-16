using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler
        (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
        : INotificationHandler<OrderCreatedEvent>//consumes/handles domain events published from Infrastructure layer
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)//when mediator.publish is called from infra layer, this handle method is called
        {//once our event is published, we will get a notification here
            logger.LogInformation("Domain event handled : {DomainEvent}", domainEvent.GetType().Name);

            if (await featureManager.IsEnabledAsync("OrderFullfilment"))//OrderFullfilment is defined in appsetings.json. OrderFullfilment is false for seeding of data or else it's true
            {//publish events only if OrderFullfilment is true i.e it does not come from seeding
                OrderDto? orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
                //publish integration event here
                await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);//publish orderCreatedIntegrationEvent to rabbitMQ
            }
        }
    }
}
