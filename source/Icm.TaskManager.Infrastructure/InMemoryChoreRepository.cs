using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.ChoreManager.Domain;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;
using static System.Threading.Tasks.Task;

namespace Icm.ChoreManager.Infrastructure
{
    public class InMemoryChoreRepository : IChoreRepository, IEnumerable<ChoreMemento>
    {
        private InMemoryChoreRepository(IDictionary<ChoreId, ChoreMemento> storage)
        {
            this.storage = storage;
        }

        public static InMemoryChoreRepository WithStaticStorage()
        {
            return new InMemoryChoreRepository(StaticStorage);
        }

        public static InMemoryChoreRepository WithInstanceStorage()
        {
            return new InMemoryChoreRepository(new Dictionary<ChoreId, ChoreMemento>());
        }

        private static readonly object StorageLock = new object();
        private static readonly IDictionary<ChoreId, ChoreMemento> StaticStorage = new Dictionary<ChoreId, ChoreMemento>();
        private readonly IDictionary<ChoreId, ChoreMemento> storage;

        private readonly List<Operation> transactionLog = new List<Operation>();

        public Task<ChoreId> AddAsync(Chore item)
        {
            var newId = Guid.NewGuid();
            transactionLog.Add(new AddOperation(item.ToMemento(newId)));
            return FromResult<ChoreId>(newId);
        }

        public Task<Identified<ChoreId, Chore>> GetByIdAsync(ChoreId id)
        {
            return FromResult(Identified.Create(id, Chore.FromMemento(storage[id])));
        }

        public Task UpdateAsync(Identified<ChoreId, Chore> identifiedChore)
        {
            transactionLog.Add(new UpdateOperation(identifiedChore.Id, identifiedChore.ToMemento()));
            return CompletedTask;
        }

        public Task DeleteAsync(ChoreId key)
        {
            transactionLog.Add(new DeleteOperation(key));
            return CompletedTask;
        }

        public Task SaveAsync()
        {
            lock (StorageLock)
            {
                foreach (var logEntry in transactionLog)
                {
                    logEntry.Execute(storage);
                }
            }

            transactionLog.Clear();
            return CompletedTask;
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveRemindersAsync()
        {
            return FromResult(
                storage.Values
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

        private bool disposed;

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;
        }

        public IEnumerator<ChoreMemento> GetEnumerator()
        {
            return storage.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private abstract class Operation
        {
            public abstract void Execute(IDictionary<ChoreId, ChoreMemento> storage);
        }

        private class AddOperation : Operation
        {
            private readonly ChoreMemento memento;

            public AddOperation(ChoreMemento memento)
            {
                this.memento = memento;
            }

            public override void Execute(IDictionary<ChoreId, ChoreMemento> storage)
            {
                storage[memento.Id] = memento;
            }
        }

        private class UpdateOperation : Operation
        {
            private readonly ChoreId id;
            private readonly ChoreMemento memento;

            public UpdateOperation(ChoreId id, ChoreMemento memento)
            {
                this.id = id;
                this.memento = memento;
            }
            public override void Execute(IDictionary<ChoreId, ChoreMemento> storage)
            {
                storage[id] = memento;
            }
        }


        private class DeleteOperation : Operation
        {
            private readonly ChoreId id;

            public DeleteOperation(ChoreId id)
            {
                this.id = id;
            }

            public override void Execute(IDictionary<ChoreId, ChoreMemento> storage)
            {
                storage.Remove(id);
            }
        }
    }
}