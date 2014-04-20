using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class DueDateRecurrence : Recurrence
    {
        public override Task CreateRecurringTask(Task task, ICurrentDateProvider currentDateProvider)
        {
            Instant dueDate = task.DueDate.Plus(task.Recurrence.RepeatInterval);
            Task newTask = task.CopyWithNewDueDate(dueDate, currentDateProvider);
            return newTask;
        }
    }
}