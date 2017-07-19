using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tasks.Icm.TaskManager.Domain.Tasks;
using static System.Threading.Tasks.Task;
using Task = System.Threading.Tasks.Task;

namespace Icm.TaskManager.Domain
{
    public class MemoryOwnKeyRepository<TKey, TItem> : IRepository<TKey, TItem>, IEnumerable<TItem>
    {
        private readonly Func<TItem, TKey> keyFunction;

        private readonly IDictionary<TKey, TItem> store;

        public MemoryOwnKeyRepository(Func<TItem, TKey> keyFunction)
        {
            store = new Dictionary<TKey, TItem>();
            this.keyFunction = keyFunction;
        }

        public Task<TKey> Add(TItem item)
        {
            var newKey = keyFunction(item);
            store.Add(newKey, item);

            return FromResult(newKey);
        }

        public Task<Identified<TKey, TItem>> GetByIdAsync(TKey id)
        {
            return FromResult(
                store.ContainsKey(id)
                ? Identified.Create(id, store[id])
                : null);
        }

        public Task Update(Identified<TKey, TItem> identifiedChore)
        {
            store[identifiedChore.Id] = identifiedChore.Value;
            return CompletedTask;
        }

        public Task Delete(TKey key)
        {
            store.Remove(key);
            return CompletedTask;
        }

        public virtual Task Save()
        {
            return CompletedTask;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return store.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return store.Values.GetEnumerator();
        }
    }
}