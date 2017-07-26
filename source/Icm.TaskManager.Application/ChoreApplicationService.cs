using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;

namespace Icm.ChoreManager.Application
{
    public class ChoreApplicationService : IChoreApplicationService
    {
        private readonly Func<IChoreRepository> buildChoreRepository;
        private readonly IClock clock;

        public ChoreApplicationService(Func<IChoreRepository> buildChoreRepository, IClock clock)
        {
            this.buildChoreRepository = buildChoreRepository;
            this.clock = clock;
        }

        public async Task ChangeStartDateAsync(ChoreId choreId, Instant newStartDate)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.StartDate = newStartDate;
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task<ChoreId> CreateAsync(string description, Instant dueDate)
        {
            using (var repository = buildChoreRepository())
            {
                var id = await CreateAsync(description, dueDate, repository);
                await repository.SaveAsync();
                return id;

            }
        }

        public async Task<ChoreDto> GetByIdAsync(ChoreId choreId)
        {
            using (var repository = buildChoreRepository())
            {
                return (await repository.GetByIdAsync(choreId)).ToDto();
            }
        }

        public Task<IEnumerable<ChoreDto>> GetActiveChoresAsync()
        {
            using (var repository = buildChoreRepository())
            {
                //// return (await repository.GetUnfinishedChoresAsync()).ToDto();
                throw new NotImplementedException();
            }
        }

        public async Task ChangeRecurrenceToFinishDateAsync(ChoreId id, Duration repeatInterval)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Recurrence = new FinishDateRecurrence(repeatInterval);
                await choreRepository.UpdateAsync(identifiedChore);
            }
        }

        public async Task ChangeRecurrenceToDueDateAsync(ChoreId id, Duration repeatInterval)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var choreId = new ChoreId(id);
                var identifiedChore = await choreRepository.GetByIdAsync(choreId);

                identifiedChore.Value.Recurrence = new DueDateRecurrence(repeatInterval);
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task StartAsync(ChoreId choreId)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);
                identifiedChore.Value.StartDate = clock.GetCurrentInstant();
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task<ChoreId?> FinishAsync(ChoreId choreId)
        {
            using (var choreRepository = buildChoreRepository())
            {
                Instant finishInstant = clock.GetCurrentInstant();
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.FinishDate = finishInstant;
                var recurringChore = identifiedChore.Value.Recurrence.Match(
                    recurrence => recurrence.CreateRecurringChore(identifiedChore.Value, finishInstant));

                await choreRepository.UpdateAsync(identifiedChore);
                if (recurringChore == null)
                {
                    return null;
                }

                var recurringChoreId = await choreRepository.AddAsync(recurringChore);
                await choreRepository.SaveAsync();
                return recurringChoreId;
            }
        }

        public async Task ChangeDescriptionAsync(ChoreId choreId, string newDescription)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Description = newDescription;
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task ChangePriorityAsync(ChoreId choreId, int newPriority)
        {
            using (var choreRepository = buildChoreRepository())
            {
                await ChangePriorityAsync(choreId, newPriority, choreRepository);
            }
        }

        public async Task ChangeDueDateAsync(ChoreId choreId, Instant newDueDate)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.DueDate = newDueDate;
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task ChangeLabelsAsync(ChoreId choreId, string newLabels)
        {
            using (var choreRepository = buildChoreRepository())
            {
                await ChangeLabelsAsync(choreId, newLabels, choreRepository);
            }
        }

        public async Task ChangeNotesAsync(ChoreId choreId, string newNotes)
        {
            using (var choreRepository = buildChoreRepository())
            {
                await ChangeNotesAsync(choreId, newNotes, choreRepository);
            }
        }

        public async Task AddReminderAsync(ChoreId choreId, Instant reminder)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Reminders.Add(reminder);
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<TimeDto>> PendingTimesAsync()
        {
            using (var choreRepository = buildChoreRepository())
            {
                var activeReminders = await choreRepository.GetActiveRemindersAsync();
                return activeReminders
                    .Select(t => new TimeDto(t.Item1, t.Item2));
            }
        }

        private async Task<ChoreId> CreateAsync(string description, Instant dueDate, IChoreRepository choreRepository)
        {
            var creationInstant = clock.GetCurrentInstant();

            var chore = Chore.Create(
                description,
                dueDate,
                creationInstant);

            return await choreRepository.AddAsync(chore);
        }

        private static async Task ChangePriorityAsync(ChoreId choreId, int newPriority, IChoreRepository choreRepository)
        {
            var id = new ChoreId(choreId);
            var identifiedChore = await choreRepository.GetByIdAsync(id);

            identifiedChore.Value.Priority = newPriority;
            await choreRepository.UpdateAsync(identifiedChore);
            await choreRepository.SaveAsync();
        }

        private static async Task ChangeLabelsAsync(ChoreId choreId, string newLabels, IChoreRepository choreRepository)
        {
            var id = new ChoreId(choreId);
            var identifiedChore = await choreRepository.GetByIdAsync(id);

            identifiedChore.Value.Labels = newLabels;
            await choreRepository.UpdateAsync(identifiedChore);
            await choreRepository.SaveAsync();
        }

        private static async Task ChangeNotesAsync(ChoreId choreId, string newNotes, IChoreRepository choreRepository)
        {
            var id = new ChoreId(choreId);
            var identifiedChore = await choreRepository.GetByIdAsync(id);

            identifiedChore.Value.Notes = newNotes;
            await choreRepository.UpdateAsync(identifiedChore);
            await choreRepository.SaveAsync();
        }
    }
}
