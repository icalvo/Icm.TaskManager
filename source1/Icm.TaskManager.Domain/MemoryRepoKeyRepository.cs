using System;
using System.Collections;
using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;

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

        public TKey Add(TItem item)
        {
            lastKey = keyGenerator(lastKey);
            Store.Add(lastKey, item);

            return lastKey;
        }

        public Identified<TKey, TItem> GetById(TKey id)
        {
            if (Store.ContainsKey(id))
            {
                return IdentifiedTools.Identified(id, Store[id]);
            }

            return null;
        }

        public void Update(Identified<TKey, TItem> value)
        {
            Store[value.Id] = value.Value;
        }

        public void Delete(TKey key)
        {
            Store.Remove(key);
        }

        public virtual void Save()
        {
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return Store.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Store.Values.GetEnumerator();
        }
    }
}