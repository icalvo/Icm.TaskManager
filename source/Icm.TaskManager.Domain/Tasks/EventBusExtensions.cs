using Icm.TaskManager.Infrastructure.Interfaces;

namespace Icm.TaskManager.Domain.Tasks
{
    public static class EventBusExtensions
    {
        public static void Subscribe<TEvent>(this IEventBus eventBus, IEventHandler<TEvent> handler)
        {
            eventBus.Subscribe(
                ev => ev is TEvent,
                ev => handler.HandleAsync((TEvent)ev));
        }
    }
}