using NodaTime;

namespace Icm.TaskManager.Domain.Chores
{
    public class DueDateRecurrence : Recurrence
    {
        public DueDateRecurrence(Duration repeatInterval)
            : base(repeatInterval)
        {
        }

        public override Chore CreateRecurringTask(Chore chore, Instant now)
        {
            Instant dueDate = chore.DueDate + RepeatInterval;
            return chore.CopyWithNewDueDate(dueDate, now);
        }
    }
}