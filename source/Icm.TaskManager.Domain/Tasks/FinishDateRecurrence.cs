using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class FinishDateRecurrence : Recurrence
    {
        public override Task CreateRecurringTask(Task task, ICurrentDateProvider currentDateProvider)
        {
            Task newTask = null;
            if (task.FinishDate.HasValue)
            {
                Instant dueDate = task.FinishDate.Value.Plus(task.Recurrence.RepeatInterval);
                newTask = task.CopyWithNewDueDate(dueDate, currentDateProvider);
            }

            return newTask;
        }
    }
}