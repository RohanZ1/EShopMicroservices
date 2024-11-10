namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger ) 
        : INotificationHandler<OrderCreatedEvent>//consumes/handles domain events published from Infrastructure layer
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)//when mediator.publish is called from infra layer, this handle method is called
        {//once our event is published, we will get a notification here
           logger.LogInformation("Domain event handled : {DomainEvent}",notification.GetType().Name);

        //publish integration event here
            return Task.CompletedTask;
        }
    }
}
