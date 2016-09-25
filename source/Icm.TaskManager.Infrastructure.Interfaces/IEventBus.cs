using System;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface IEventBus
    {
        void Subscribe(
            Func<object, bool> condition,
            Func<object, System.Threading.Tasks.Task> handler);

        void Publish<TEvent>(TEvent reminderElapsed);
    }
}