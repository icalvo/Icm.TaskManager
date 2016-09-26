using System;
using Edument.CQRS;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Infrastructure.Interfaces;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class TaskApplicationService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IClock clock;
        private readonly IEventBus eventBus;
        private readonly IMessageDispatcher messageDispatcher;

        public TaskApplicationService(ITaskRepository taskRepository, IClock clock, IEventBus eventBus, IMessageDispatcher messageDispatcher)
        {
            this.taskRepository = taskRepository;
            this.clock = clock;
            this.eventBus = eventBus;
            this.messageDispatcher = messageDispatcher;
        }

        public int CreateTask(
            string description,
            Instant dueDate,
            int priority,
            string notes,
            string labels)
        {
            var id = CreateSimpleTask(description, dueDate);

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

        public int CreateFinishDateRecurringTask(
            string description,
            Instant dueDate,
            Duration repeatInterval,
            int priority,
            string notes,
            string labels)
        {
            var id = CreateTask(description, dueDate, priority, notes, labels);

            ChangeRecurrenceToFinishDate(id, repeatInterval);

            return id;
        }

        public int CreateSimpleTask(string description, Instant dueDate)
        {
            var creationInstant = clock.Now;

            messageDispatcher.SendCommand(new CreateTask());

            var task = Task.Create(
                description,
                dueDate,
                creationInstant);

            var id = taskRepository.Add(task);

            var id2 = Guid.NewGuid();
            eventBus.Publish(new TaskCreatedEvent(id2, description, dueDate, creationInstant));
            return id;
        }

        public void ChangeRecurrenceToFinishDate(int id, Duration repeatInterval)
        {
            var taskId = new TaskId(id);
            var task = taskRepository.GetById(taskId);

            task.Recurrence = new FinishDateRecurrence(repeatInterval);
            taskRepository.Update(taskId, task);
            taskRepository.Save();
        }

        public void ChangeRecurrenceToDueDate(int id, Duration repeatInterval)
        {
            var taskId = new TaskId(id);
            var task = taskRepository.GetById(taskId);

            task.Recurrence = new DueDateRecurrence(repeatInterval);
            taskRepository.Update(taskId, task);
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
            var task = taskRepository.GetById(taskId);

            if (recurrenceType != null && repeatInterval.HasValue)
            {
                task.Recurrence = Recurrence.FromType(recurrenceType, repeatInterval.Value);
            }

            taskRepository.Update(taskId, task);
            taskRepository.Save();
            return id;
        }

        public void StartTask(int taskId)
        {
            var id = new TaskId(taskId);
            var task = taskRepository.GetById(id);
            task.StartDate = clock.Now;
            taskRepository.Update(id, task);
            taskRepository.Save();
        }

        public int? FinishTask(int taskId)
        {
            Instant finishInstant = clock.Now;
            var id = new TaskId(taskId);
            var task = taskRepository.GetById(id);

            task.FinishDate = finishInstant;
            var recurringTask = task.Recurrence.Match(
                recurrence => recurrence.CreateRecurringTask(task, finishInstant));

            taskRepository.Update(id, task);
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
            var task = taskRepository.GetById(id);

            task.Description = newDescription;
            taskRepository.Update(id, task);
            taskRepository.Save();
        }

        public void ChangeTaskPriority(int taskId, int newPriority)
        {
            var id = new TaskId(taskId);
            var task = taskRepository.GetById(id);

            task.Priority = newPriority;
            taskRepository.Update(id, task);
            taskRepository.Save();
        }

        public void ChangeTaskLabels(int taskId, string newLabels)
        {
            var id = new TaskId(taskId);
            var task = taskRepository.GetById(id);

            task.Labels = newLabels;
            taskRepository.Update(id, task);
            taskRepository.Save();
        }

        public void ChangeTaskNotes(int taskId, string newNotes)
        {
            var id = new TaskId(taskId);
            var task = taskRepository.GetById(id);

            task.Notes = newNotes;
            taskRepository.Update(id, task);
            taskRepository.Save();

            eventBus.Publish(new TaskNotesChangedEvent(taskId, newNotes));
        }

        public void AddTaskReminder(int taskId, Instant reminder)
        {
            var id = new TaskId(taskId);
            var task = taskRepository.GetById(id);

            task.Reminders.Add(reminder);
            taskRepository.Update(id, task);
            taskRepository.Save();

            eventBus.Publish(new ReminderAddedEvent(taskId, reminder));
        }

        public void AddTaskReminderRelativeToNow(int taskId, Duration offset)
        {
            AddTaskReminder(taskId, clock.Now.Plus(offset));
        }
    }
}
