using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class DueDateRecurrence : Recurrence
    {
        public DueDateRecurrence(Duration repeatInterval)
            : base(repeatInterval)
        {
        }

        public override Task CreateRecurringTask(Task task, Instant now)
        {
            Instant dueDate = task.DueDate + RepeatInterval;
            return task.CopyWithNewDueDate(dueDate, now);
        }
    }
}