using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icm.TaskManager.Domain.Chores;
using Icm.TaskManager.Domain.Chores.Icm.TaskManager.Domain.Tasks;
using static System.Threading.Tasks.Task;
using Task = System.Threading.Tasks.Task;

namespace Icm.TaskManager.Domain
{
    public class MemoryRepoKeyRepository<TKey, TItem> : IRepository<TKey, TItem>, IEnumerable<TItem>
    {
        private readonly Func<TKey, TKey> keyGenerator;
        private TKey lastKey;

        protected IDictionary<TKey, TItem> Store { get; }

        public MemoryRepoKeyRepository(IDictionary<TKey, TItem> initialElements, Func<TKey, TKey> keyGenerator)
        {
            Store = new Dictionary<TKey, TItem>();
            foreach (var initialElement in initialElements)
            {
                Store.Add(initialElement.Key, initialElement.Value);
            }

            this.keyGenerator = keyGenerator;
        }

        public MemoryRepoKeyRepository(Func<TKey, TKey> keyGenerator)
        {
            Store = new Dictionary<TKey, TItem>();
            this.keyGenerator = keyGenerator;
        }

        public Task<TKey> Add(TItem item)
        {
            lastKey = keyGenerator(lastKey);
            Store.Add(lastKey, item);

            return FromResult(lastKey);
        }

        public Task<Identified<TKey, TItem>> GetByIdAsync(TKey id)
        {
            if (Store.ContainsKey(id))
            {
                return FromResult(Identified.Create(id, Store[id]));
            }

            return FromResult<Identified<TKey, TItem>>(null);
        }

        public Task Update(Identified<TKey, TItem> identifiedChore)
        {
            Store[identifiedChore.Id] = identifiedChore.Value;
            return CompletedTask;
        }

        public Task Delete(TKey key)
        {
            Store.Remove(key);
            return CompletedTask;
        }

        public virtual Task Save()
        {
            return CompletedTask;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return Store.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Store.Values.GetEnumerator();
        }

        public void Dispose()
        {
        }
    }
}