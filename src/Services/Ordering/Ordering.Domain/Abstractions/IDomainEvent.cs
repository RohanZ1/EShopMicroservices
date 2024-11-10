

namespace Ordering.Domain.Abstractions
{
    public interface IDomainEvent : INotification//INotification allows DomainEvent to be dispatched through mediatr handlers
    {
        Guid EventId=> Guid.NewGuid();
        public DateTime OccuredOn => DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName!;//AssemblyQualifiedName of the class which is throwing this IDomainEvent
    }
}
