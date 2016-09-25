using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class DueDateRecurrence : Recurrence
    {
        public override Task CreateRecurringTask(Task task, Instant now)
        {
            return MonadicExtensions.Match(
                task.Recurrence,
                recurrence =>
                {
                    Instant dueDate = task.DueDate + recurrence.RepeatInterval;
                    return task.CopyWithNewDueDate(dueDate, now);
                });
        }
    }
}