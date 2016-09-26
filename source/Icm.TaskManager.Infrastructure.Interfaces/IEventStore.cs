using System;
using System.Collections;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface IEventStore
    {
        IEnumerable LoadEventsFor<TAggregate>(Guid id);

        void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, ArrayList newEvents);
    }
}
