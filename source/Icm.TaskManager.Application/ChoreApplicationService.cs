using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.TaskManager.Domain.Chores;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public interface IChoreRepositoryFactory
    {
        IChoreRepository Build();
    }

    public class ChoreApplicationService : IChoreApplicationService
    {
        private readonly Func<IChoreRepository> buildChoreRepository;
        private readonly IClock clock;

        public ChoreApplicationService(Func<IChoreRepository> buildChoreRepository, IClock clock)
        {
            this.buildChoreRepository = buildChoreRepository;
            this.clock = clock;
        }

        public async Task<Guid> Create(
            string description,
            Instant dueDate,
            int priority,
            string notes,
            string labels)
        {
            using (var repository = buildChoreRepository())
            {

                var id = await Create(description, dueDate, repository);

                await ChangePriority(id, priority, repository);
                await ChangeNotes(id, notes, repository);
                await ChangeLabels(id, labels, repository);
                await repository.Save();
                return id;
            }
        }

        public async Task<Guid> CreateDueDateRecurringTask(
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

        public async Task ChangeStartDate(Guid taskId, Instant newStartDate)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(taskId);
                var idtask = await choreRepository.GetByIdAsync(id);

                idtask.Value.StartDate = newStartDate;
                await choreRepository.Update(idtask);
                await choreRepository.Save();
            }
        }

        public async Task<Guid> Create(string description, Instant dueDate)
        {
            using (var repository = buildChoreRepository())
            {
                var id = await Create(description, dueDate, repository);
                await repository.Save();
                return id;

            }
        }

        public async Task<ChoreDto> GetById(Guid choreId)
        {
            using (var repository = buildChoreRepository())
            {
                return (await repository.GetByIdAsync(choreId)).ToDto();
            }
        }

        public Task<IEnumerable<ChoreDto>> GetChoresFrom(Guid choreId)
        {
            using (var repository = buildChoreRepository())
            {
                ////return (await repository.GetChores(choreId)).ToDto();
                throw new NotImplementedException();
            }
        }

        public async Task ChangeRecurrenceToFinishDate(Guid id, Duration repeatInterval)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var idtask = await choreRepository.GetByIdAsync(id);

                idtask.Value.Recurrence = new FinishDateRecurrence(repeatInterval);
                await choreRepository.Update(idtask);
            }
        }

        public async Task ChangeRecurrenceToDueDate(Guid id, Duration repeatInterval)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var taskId = new ChoreId(id);
                var idtask = await choreRepository.GetByIdAsync(taskId);

                idtask.Value.Recurrence = new DueDateRecurrence(repeatInterval);
                await choreRepository.Update(idtask);
                await choreRepository.Save();
            }
        }

        public async Task Start(Guid taskId)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(taskId);
                var idtask = await choreRepository.GetByIdAsync(id);
                idtask.Value.StartDate = clock.GetCurrentInstant();
                await choreRepository.Update(idtask);
                await choreRepository.Save();
            }
        }

        public async Task<Guid?> Finish(Guid taskId)
        {
            using (var choreRepository = buildChoreRepository())
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
        }

        public async Task ChangeDescription(Guid taskId, string newDescription)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(taskId);
                var idtask = await choreRepository.GetByIdAsync(id);

                idtask.Value.Description = newDescription;
                await choreRepository.Update(idtask);
                await choreRepository.Save();
            }
        }

        public async Task ChangePriority(Guid taskId, int newPriority)
        {
            using (var choreRepository = buildChoreRepository())
            {
                await ChangePriority(taskId, newPriority, choreRepository);
            }
        }

        public async Task ChangeDueDate(Guid taskId, Instant newDueDate)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(taskId);
                var idtask = await choreRepository.GetByIdAsync(id);

                idtask.Value.DueDate = newDueDate;
                await choreRepository.Update(idtask);
                await choreRepository.Save();
            }
        }

        public async Task ChangeLabels(Guid taskId, string newLabels)
        {
            using (var choreRepository = buildChoreRepository())
            {
                await ChangeLabels(taskId, newLabels, choreRepository);
            }
        }

        public async Task ChangeNotes(Guid taskId, string newNotes)
        {
            using (var choreRepository = buildChoreRepository())
            {
                await ChangeNotes(taskId, newNotes, choreRepository);
            }
        }

        public async Task AddReminder(Guid taskId, Instant reminder)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(taskId);
                var idtask = await choreRepository.GetByIdAsync(id);

                idtask.Value.Reminders.Add(reminder);
                await choreRepository.Update(idtask);
                await choreRepository.Save();
            }
        }

        public async Task<IEnumerable<TimeDto>> PendingTimes()
        {
            using (var choreRepository = buildChoreRepository())
            {
                var activeReminders = await choreRepository.GetActiveReminders();
                return activeReminders
                    .Select(t => new TimeDto(t.Item1, t.Item2));
            }
        }

        private async Task<Guid> Create(string description, Instant dueDate, IChoreRepository choreRepository)
        {
            var creationInstant = clock.GetCurrentInstant();

            var task = Chore.Create(
                description,
                dueDate,
                creationInstant);

            var id = await choreRepository.Add(task);

            return id;
        }

        private static async Task ChangePriority(Guid taskId, int newPriority, IChoreRepository choreRepository)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Priority = newPriority;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        private static async Task ChangeLabels(Guid taskId, string newLabels, IChoreRepository choreRepository)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Labels = newLabels;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }

        private static async Task ChangeNotes(Guid taskId, string newNotes, IChoreRepository choreRepository)
        {
            var id = new ChoreId(taskId);
            var idtask = await choreRepository.GetByIdAsync(id);

            idtask.Value.Notes = newNotes;
            await choreRepository.Update(idtask);
            await choreRepository.Save();
        }
    }
}
