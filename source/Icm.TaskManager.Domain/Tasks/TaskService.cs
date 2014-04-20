using System;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ICurrentDateProvider currentDateProvider;

        public TaskService(ICurrentDateProvider currentDateProvider)
        {
            this.currentDateProvider = currentDateProvider;
        }

        public Task Finish(Task task)
        {
            Task recurringTask = null;
            if (task.IsDone)
            {
                throw new TaskAlreadyDoneException();
            }

            task.SetFinishDate(currentDateProvider.Now);
            if (task.Recurrence != null)
            {
                recurringTask = task.Recurrence.CreateRecurringTask(task, this.currentDateProvider);
            }

            return recurringTask;
        }

        public Task CreateTask(string description, Instant? startDate, Instant dueDate, string recurrenceType, Duration? repeatInterval, int priority, string notes, string labels)
        {
            var newTask = new Task();

            newTask.Description = description;
            newTask.CreationDate = currentDateProvider.Now;
            newTask.DueDate = dueDate;
            newTask.RecurrenceType = recurrenceType;
            newTask.RepeatInterval = repeatInterval;
            newTask.Priority = priority;
            newTask.Notes = notes;
            newTask.Labels = labels;

            return newTask;
        }
    }
}
