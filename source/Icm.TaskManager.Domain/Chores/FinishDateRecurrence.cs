using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public class FinishDateRecurrence : Recurrence
    {
        public FinishDateRecurrence(Duration repeatInterval)
            : base(repeatInterval)
        {
        }

        public override Chore CreateRecurringChore(Chore chore, Instant now)
        {
            return MonadicExtensions.Match(
                chore.FinishDate,
                finishDate =>
                {
                    Instant dueDate = finishDate + RepeatInterval;
                    return chore.CopyWithNewDueDate(dueDate, now);
                });
        }
    }
}