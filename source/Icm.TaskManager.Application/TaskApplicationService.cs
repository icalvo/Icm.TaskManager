using System;
using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class TaskApplicationService : ITaskApplicationService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IClock clock;

        public TaskApplicationService(ITaskRepository taskRepository, IClock clock)
        {
            this.taskRepository = taskRepository;
            this.clock = clock;
        }

        public int CreateTask(
            string description,
            Instant dueDate,
            int priority,
            string notes,
            string labels)
        {
            var id = CreateTask(description, dueDate);

            ChangeTaskPriority(id, priority);
            ChangeTaskNotes(id, notes);
            ChangeTaskLabels(id, labels);
            return id;
        }

        public int CreateDueDateRecurringTask(
            string description,
            Instant dueDate,
            Duration repeatInterval,
            int priority,
            string notes,
            string labels)
        {
            var id = CreateTask(description, dueDate, priority, notes, labels);

            ChangeRecurrenceToDueDate(id, repeatInterval);

            return id;
        }

        public int CreateTask(string description, Instant dueDate)
        {
            var creationInstant = clock.GetCurrentInstant();

            var task = Task.Create(
                description,
                dueDate,
                creationInstant);

            var id = taskRepository.Add(task);

            return id;
        }

        public TaskDto GetTaskById(int taskId)
        {
            return taskRepository.GetById(taskId).ToDto();
        }

        public IEnumerable<TaskDto> GetTasks()
        {
            throw new NotImplementedException();
        }

        public void ChangeRecurrenceToFinishDate(int id, Duration repeatInterval)
        {
            var idtask = taskRepository.GetById(id);

            idtask.Value.Recurrence = new FinishDateRecurrence(repeatInterval);
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public void ChangeRecurrenceToDueDate(int id, Duration repeatInterval)
        {
            var taskId = new TaskId(id);
            var idtask = taskRepository.GetById(taskId);

            idtask.Value.Recurrence = new DueDateRecurrence(repeatInterval);
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public int CreateTaskParsing(
            string description,
            Instant dueDate,
            string recurrenceType,
            Duration? repeatInterval,
            int priority,
            string notes,
            string labels)
        {
            var id = CreateTask(description, dueDate, priority, notes, labels);

            var taskId = new TaskId(id);
            var idtask = taskRepository.GetById(taskId);

            if (recurrenceType != null && repeatInterval.HasValue)
            {
                idtask.Value.Recurrence = Recurrence.FromType(recurrenceType, repeatInterval.Value);
            }

            taskRepository.Update(idtask);
            taskRepository.Save();
            return id;
        }

        public void StartTask(int taskId)
        {
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);
            idtask.Value.StartDate = clock.GetCurrentInstant();
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public int? FinishTask(int taskId)
        {
            Instant finishInstant = clock.GetCurrentInstant();
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);

            idtask.Value.FinishDate = finishInstant;
            var recurringTask = idtask.Value.Recurrence.Match(
                recurrence => recurrence.CreateRecurringTask(idtask.Value, finishInstant));

            taskRepository.Update(idtask);
            if (recurringTask == null)
            {
                return null;
            }

            var recurringTaskId = taskRepository.Add(recurringTask);
            taskRepository.Save();
            return recurringTaskId;
        }

        public void ChangeTaskDescription(int taskId, string newDescription)
        {
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);

            idtask.Value.Description = newDescription;
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public void ChangeTaskPriority(int taskId, int newPriority)
        {
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);

            idtask.Value.Priority = newPriority;
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public void ChangeTaskLabels(int taskId, string newLabels)
        {
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);

            idtask.Value.Labels = newLabels;
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public void ChangeTaskNotes(int taskId, string newNotes)
        {
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);

            idtask.Value.Notes = newNotes;
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public void AddTaskReminder(int taskId, Instant reminder)
        {
            var id = new TaskId(taskId);
            var idtask = taskRepository.GetById(id);

            idtask.Value.Reminders.Add(reminder);
            taskRepository.Update(idtask);
            taskRepository.Save();
        }

        public void AddTaskReminderRelativeToNow(int taskId, Duration offset)
        {
            AddTaskReminder(taskId, clock.GetCurrentInstant().Plus(offset));
        }
    }
}
