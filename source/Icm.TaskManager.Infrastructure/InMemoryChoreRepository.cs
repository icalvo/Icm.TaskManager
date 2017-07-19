using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.TaskManager.Domain;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tasks.Icm.TaskManager.Domain.Tasks;
using NodaTime;
using static System.Threading.Tasks.Task;

namespace Icm.TaskManager.Infrastructure
{
    public class InMemoryChoreRepository : IChoreRepository
    {
        private static readonly Dictionary<ChoreId, ChoreMemento> Storage = new Dictionary<ChoreId, ChoreMemento>();

        public Task<ChoreId> Add(Chore item)
        {
            var newId = Storage.Keys.Any() ? Storage.Keys.Max(x => x.Value) + 1 : 1;
            Storage.Add(newId, item.ToMemento(newId));

            return FromResult<ChoreId>(newId);
        }

        public Task<Identified<ChoreId, Chore>> GetByIdAsync(ChoreId id)
        {
            return FromResult(Identified.Create(id, Chore.FromMemento(Storage[id])));
        }

        public Task Update(Identified<ChoreId, Chore> identifiedChore)
        {
            Storage[identifiedChore.Id] = identifiedChore.ToMemento();
            return CompletedTask;
        }

        public Task Delete(ChoreId key)
        {
            Storage.Remove(key);
            return CompletedTask;
        }

        public Task Save()
        {
            return CompletedTask;
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveReminders()
        {
            return FromResult(
                Storage.Values
                .SelectMany(GetActiveTimes));
        }

        private static IEnumerable<(Instant Time, TimeKind Kind)> GetActiveTimes(ChoreMemento chore)
        {
            if (chore.StartDate.HasValue)
            {
                yield return (chore.StartDate.Value, TimeKind.StartDate);
            }

            yield return (chore.DueDate, TimeKind.DueDate);

            if (chore.FinishDate.HasValue)
            {
                yield return (chore.FinishDate.Value, TimeKind.FinishDate);
            }

            foreach (var reminder in chore.Reminders)
            {
                yield return (reminder, TimeKind.Reminder);
            }
        }
    }
}