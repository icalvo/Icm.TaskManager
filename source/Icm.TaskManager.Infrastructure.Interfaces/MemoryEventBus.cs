using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public class MemoryEventBus : IEventBus
    {
        private static readonly ConcurrentBag<Tuple<Func<object, bool>, Func<object, Task>>> Handlers;
        private static readonly BlockingCollection<object> Events;

        static MemoryEventBus()
        {
            Handlers = new ConcurrentBag<Tuple<Func<object, bool>, Func<object, Task>>>();
            Events = new BlockingCollection<object>();

            Task.Run(() =>
            {
                foreach (var @event in Events.GetConsumingEnumerable())
                {
                    Parallel.ForEach(
                        Handlers.Where(handler => handler.Item1(@event)),
                        handler => handler.Item2(@event));
                }
            });
        }

        public void Subscribe(Func<object, bool> condition, Func<object, Task> handler)
        {
            Handlers.Add(Tuple.Create(condition, handler));
        }

        public void Publish<TEvent>(TEvent @event)
        {
            Events.Add(@event);
        }
    }
}