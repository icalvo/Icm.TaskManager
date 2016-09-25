using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class FinishDateRecurrence : Recurrence
    {
        public override Task CreateRecurringTask(Task task, Instant now)
        {
            return MonadicExtensions.Match(
                task.FinishDate,
                task.Recurrence,
                (finishDate, recurrence) =>
                {
                    Instant dueDate = finishDate + recurrence.RepeatInterval;
                    return task.CopyWithNewDueDate(dueDate, now);
                });
        }
    }
}