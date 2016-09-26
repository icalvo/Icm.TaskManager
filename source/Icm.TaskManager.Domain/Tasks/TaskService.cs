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

        public Task CreateTask(string description, Instant dueDate)
        {
            return Task.Create(
                description,
                dueDate,
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
