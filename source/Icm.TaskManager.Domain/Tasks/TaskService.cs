using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IClock clock;

        public TaskService(IClock clock)
        {
            this.clock = clock;
        }

        public Task CreateTask(string description, Instant? startDate, Instant dueDate, string recurrenceType, Duration? repeatInterval, int priority, string notes, string labels)
        {
            return Task.Create(
                description,
                startDate,
                dueDate,
                Recurrence.FromType(recurrenceType),
                repeatInterval,
                priority,
                notes,
                labels,
                clock.Now);
        }

        public Task Finish(Task task)
        {
            return Finish(task, clock.Now);
        }

        public Task Finish(Task task, Instant finishDate)
        {
            task.FinishDate = finishDate;
            return task.Recurrence.Match(
                recurrence => recurrence.CreateRecurringTask(task, finishDate));
        }
    }
}
