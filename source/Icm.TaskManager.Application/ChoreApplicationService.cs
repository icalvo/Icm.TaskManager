using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class ChoreApplicationService : IChoreApplicationService
    {
        private readonly IChoreRepository choreRepository;
        private readonly IClock clock;

        public ChoreApplicationService(IChoreRepository choreRepository, IClock clock)
        {
            this.choreRepository = choreRepository;
            this.clock = clock;
        }

        public async Task<int> Create(
            string description,
            Instant dueDate,
            int priority,
            string notes,
            string labels)
        {
            var id = await Create(description, dueDate);

            await ChangePriority(id, priority);
            await ChangeNotes(id, notes);
            await ChangeLabels(id, labels);
            return id;
        }

        public async Task<int> CreateDueDateRecurringTask(
            string description,
            Instant dueDate,
            Duration repeatInterval,
            int priority,
            string notes,
            string labels)
        {
            var id = await Create(description, dueDate, priority, notes, labels);

            await ChangeRecurrenceToDueDate(id, repeatInterval);

            return id;
        }

        public Task ChangeStartDate(int taskId, Instant newStartDate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(string description, Instant dueDate)
        {
            var creationInstant = clock.GetCurrentInstant();

            var task = Chore.Create(
                description,
                dueDate,
                creationInstant);

            var id = await choreRepository.Add(task);

            return id;
        }

        public async Task<TaskDto> GetById(int taskId)
        {
            var result = await choreRepository.GetByIdAsync(taskId);
            
            return result.ToDto();
        }

        public Task<IEnumerable<TaskDto>> GetTasks(int taskIdFrom)
        {
            throw new NotImplementedException();
        }

        public async Task ChangeRecurrenceToFinishDate(int id, Duration repeatInterval)
        {
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Recurrence = new FinishDateRecurrence(repeatInterval);
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task ChangeRecurrenceToDueDate(int id, Duration repeatInterval)
        {
            var taskId = new ChoreId(id);
            var idtask = await choreRepository.GetByIdAsync(taskId);

            idtask.Value.Recurrence = new DueDateRecurrence(repeatInterval);
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task<int> CreateParsing(
            string description,
            Instant dueDate,
            string recurrenceType,
            Duration? repeatInterval,
            int priority,
            string notes,
            string labels)
        {
            var id = await Create(description, dueDate, priority, notes, labels);

            var taskId = new ChoreId(id);
            var idtask = await choreRepository.GetByIdAsync(taskId);

            if (recurrenceType != null && repeatInterval.HasValue)
            {
                idtask.Value.Recurrence = Recurrence.FromType(recurrenceType, repeatInterval.Value);
            }

            await choreRepository.Update(idtask);
            await choreRepository.Save();
            return id;
        }

        public async Task Start(int taskId)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);
            idtask.Value.StartDate = clock.GetCurrentInstant();
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task<int?> Finish(int taskId)
        {
            Instant finishInstant = clock.GetCurrentInstant();
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.FinishDate = finishInstant;
            var recurringTask = idtask.Value.Recurrence.Match(
                recurrence => recurrence.CreateRecurringTask(idtask.Value, finishInstant));

            await choreRepository.Update(idtask);
            if (recurringTask == null)
            {
                return null;
            }

            var recurringTaskId = await choreRepository.Add(recurringTask);
            await choreRepository.Save();
            return recurringTaskId;
        }

        public async Task ChangeDescription(int taskId, string newDescription)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Description = newDescription;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task ChangePriority(int taskId, int newPriority)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Priority = newPriority;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task ChangeDueDate(int taskId, Instant newDueDate)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.DueDate = newDueDate;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task ChangeLabels(int taskId, string newLabels)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Labels = newLabels;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task ChangeNotes(int taskId, string newNotes)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Notes = newNotes;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task AddReminder(int taskId, Instant reminder)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Reminders.Add(reminder);
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        public async Task<IEnumerable<ChoreDto>> PendingTimes()
        {
            var activeReminders = await choreRepository.GetActiveReminders();
            return activeReminders
                .Select(t => new ChoreDto(t.Item1, t.Item2));
        }
    }
}
