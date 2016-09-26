using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class FinishDateRecurrence : Recurrence
    {
        public FinishDateRecurrence(Duration repeatInterval)
            : base(repeatInterval)
        {
        }

        public override Task CreateRecurringTask(Task task, Instant now)
        {
            return MonadicExtensions.Match(
                task.FinishDate,
                finishDate =>
                {
                    Instant dueDate = finishDate + RepeatInterval;
                    return task.CopyWithNewDueDate(dueDate, now);
                });
        }
    }
}