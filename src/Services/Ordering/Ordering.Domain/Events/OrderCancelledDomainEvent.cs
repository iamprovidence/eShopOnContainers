namespace Microsoft.eShopOnContainers.Services.Ordering.Domain.Events;

public interface IDomainEvent
{
}

public class OrderCancelledDomainEvent : IDomainEvent, INotification // keep INotification for backward compatibility
{
    public Order Order { get; }

    public OrderCancelledDomainEvent(Order order)
    {
        Order = order;
    }
}