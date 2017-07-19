using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class FinishDateRecurrence : Recurrence
    {
        public FinishDateRecurrence(Duration repeatInterval)
            : base(repeatInterval)
        {
        }

        public override Chore CreateRecurringTask(Chore chore, Instant now)
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