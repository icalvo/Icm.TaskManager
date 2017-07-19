using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public abstract class Recurrence
    {
        protected Recurrence(Duration repeatInterval)
        {
            RepeatInterval = repeatInterval;
        }

        public Duration RepeatInterval { get; }

        public abstract Chore CreateRecurringTask(Chore chore, Instant now);

        public static Recurrence FromType(string recurrenceType, Duration repeatInterval)
        {
            switch (recurrenceType)
            {
                case "DueDate":
                    return new DueDateRecurrence(repeatInterval);
                case "FinishDate":
                    return new FinishDateRecurrence(repeatInterval);
                default:
                    return null;
            }
        }
    }
}
