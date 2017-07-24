using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public class ChoreService : IChoreService
    {
        private readonly IClock clock;

        public ChoreService(IClock clock)
        {
            this.clock = clock;
        }

        public Chore CreateChore(string description, Instant dueDate)
        {
            return Chore.Create(
                description,
                dueDate,
                clock.GetCurrentInstant());
        }

        public Chore Finish(Chore chore)
        {
            var finishDate = clock.GetCurrentInstant();
            chore.FinishDate = finishDate;
            return chore.Recurrence.Match(
                recurrence => recurrence.CreateRecurringChore(chore, finishDate));
        }
    }
}
