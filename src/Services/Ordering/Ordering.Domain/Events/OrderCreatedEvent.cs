


namespace Ordering.Domain.Events
{
    public record OrderCreatedEvent(Order order):IDomainEvent;//IDomainEvent is a marker interface  used by infrastructure services to identify the domain events
    
}
