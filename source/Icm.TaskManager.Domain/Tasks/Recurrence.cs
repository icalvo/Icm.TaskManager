using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public abstract class Recurrence
    {
        public int Id { get; set; }

        public Duration RepeatInterval { get; internal set; }

        public abstract Task CreateRecurringTask(Task task, Instant now);

        public static Recurrence FromType(string recurrenceType)
        {
            switch (recurrenceType)
            {
                case "DueDate":
                    return new DueDateRecurrence();
                case "FinishDate":
                    return new FinishDateRecurrence();
                default:
                    return null;
            }
        }
    }
}
