using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class ChoreService : IChoreService
    {
        private readonly IClock clock;

        public ChoreService(IClock clock)
        {
            this.clock = clock;
        }

        public Chore CreateTask(string description, Instant dueDate)
        {
            return Chore.Create(
                description,
                dueDate,
                clock.GetCurrentInstant());
        }

        public Chore Finish(Chore chore)
        {
            return Finish(chore, clock.GetCurrentInstant());
        }

        public Chore Finish(Chore chore, Instant finishDate)
        {
            chore.FinishDate = finishDate;
            return chore.Recurrence.Match(
                recurrence => recurrence.CreateRecurringTask(chore, finishDate));
        }
    }
}
