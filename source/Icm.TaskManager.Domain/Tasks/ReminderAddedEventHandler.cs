using Icm.TaskManager.Infrastructure.Interfaces;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class ReminderAddedEventHandler : IEventHandler<ReminderAddedEvent>
    {
        private readonly IClock clock;
        private readonly IEventBus eventBus;

        public ReminderAddedEventHandler(IClock clock, IEventBus eventBus)
        {
            this.clock = clock;
            this.eventBus = eventBus;
        }

        public async System.Threading.Tasks.Task HandleAsync(ReminderAddedEvent ev)
        {
            var duration = ev.AlarmInstant.Minus(clock.Now);
            if (duration > Duration.Zero)
            {
                await System.Threading.Tasks.Task.Delay(duration.ToTimeSpan());
            }

            eventBus.Publish(new ReminderElapsedEvent(ev.TaskId));
        }
    }
}