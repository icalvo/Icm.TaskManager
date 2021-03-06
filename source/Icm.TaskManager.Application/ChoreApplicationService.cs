﻿using System;
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
                var creationInstant = clock.GetCurrentInstant();

                var chore = Chore.Create(
                    description,
                    dueDate,
                    creationInstant);
                var id = await repository.AddAsync(chore);
                await repository.SaveAsync();
                return id;

            }
        }

        public async Task<ChoreMemento> GetByIdAsync(ChoreId choreId)
        {
            using (var repository = buildChoreRepository())
            {
                return (await repository.GetByIdAsync(choreId)).ToMemento();
            }
        }

        public async Task<IEnumerable<ChoreMemento>> GetPendingChoresAsync()
        {
            using (var repository = buildChoreRepository())
            {
                return (await repository.GetPendingAsync()).Select(x => x.ToMemento());
            }
        }

        public async Task SetRecurrenceToFinishDateAsync(ChoreId choreId, Duration repeatInterval)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var identifiedChore = await choreRepository.GetByIdAsync(choreId);

                identifiedChore.Value.Recurrence = new FinishDateRecurrence(repeatInterval);
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task RemoveRecurrenceAsync(ChoreId choreId)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var identifiedChore = await choreRepository.GetByIdAsync(choreId);

                identifiedChore.Value.Recurrence = null;
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task SetRecurrenceToDueDateAsync(ChoreId id, Duration repeatInterval)
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
                ChoreId? recurringChoreId = null;
                if (recurringChore != null)
                {
                    recurringChoreId = await choreRepository.AddAsync(recurringChore);
                }

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
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Priority = newPriority;
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
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

        public async Task AddLabelsAsync(ChoreId choreId, string[] newLabels)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Labels = identifiedChore.Value.Labels.Union(newLabels.Select(x => x.ToLower())).ToArray();
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task RemoveLabelsAsync(ChoreId choreId, string[] labelsToRemove)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Labels = identifiedChore.Value.Labels.Except(labelsToRemove.Select(x => x.ToLower())).ToArray();
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
            }
        }

        public async Task ChangeNotesAsync(ChoreId choreId, string newNotes)
        {
            using (var choreRepository = buildChoreRepository())
            {
                var id = new ChoreId(choreId);
                var identifiedChore = await choreRepository.GetByIdAsync(id);

                identifiedChore.Value.Notes = newNotes;
                await choreRepository.UpdateAsync(identifiedChore);
                await choreRepository.SaveAsync();
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
    }
}
