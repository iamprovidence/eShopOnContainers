using Microsoft.eShopOnContainers.Services.Ordering.Domain.Events;
using Ordering.Application.Common;

namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure;

internal static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        //var domainEventNotifications = domainEvents
        //   .Select(CreateDomainEventNotification);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }


    private static INotification CreateDomainEventNotification(IDomainEvent domainEvent)
    {
        var genericType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());

        return (INotification)Activator.CreateInstance(genericType, domainEvent);
    }
}