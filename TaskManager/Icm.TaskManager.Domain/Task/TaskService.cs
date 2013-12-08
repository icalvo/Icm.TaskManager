namespace Icm.TaskManager.Domain.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ICurrentDateProvider currentDateProvider;

        public TaskService(ICurrentDateProvider currentDateProvider)
        {
            this.currentDateProvider = currentDateProvider;
        }

        public void Finish(Task task)
        {
            if (task.IsDone)
            {
                throw new TaskAlreadyDoneException();
            }

            task.SetFinishDate(currentDateProvider.Now);
        }

        public Task CreateTask(string description, System.DateTime? startDate, System.DateTime? dueDate, System.TimeSpan? repeatInterval, bool repeatFromDueDate, int priority, string notes, string labels)
        {
            var newTask = new Task();

            newTask.Description = description;
            newTask.CreationDate = currentDateProvider.Now;
            newTask.DueDate = dueDate;
            newTask.RepeatFromDueDate = repeatFromDueDate;
            newTask.RepeatInterval = repeatInterval;
            newTask.Priority = priority;
            newTask.Notes = notes;
            newTask.Labels = labels;

            return newTask;
        }
    }
}
