using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public class DueDateRecurrence : Recurrence
    {
        public DueDateRecurrence(Duration repeatInterval)
            : base(repeatInterval)
        {
        }

        public override Chore CreateRecurringChore(Chore chore, Instant now)
        {
            Instant dueDate = chore.DueDate + RepeatInterval;
            return chore.CopyWithNewDueDate(dueDate, now);
        }
    }
}