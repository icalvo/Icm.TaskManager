using System;
using System.Collections;
using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Infrastructure.Interfaces;

namespace Icm.TaskManager.Domain
{
    public class MemoryOwnKeyRepository<TKey, TItem> : IRepository<TKey, TItem>, IEnumerable<TItem>
    {
        private readonly Func<TItem, TKey> keyFunction;
        private TKey lastKey;

        protected IDictionary<TKey, TItem> Store { get; }

        public MemoryOwnKeyRepository(Func<TItem, TKey> keyFunction)
        {
            Store = new Dictionary<TKey, TItem>();
            this.keyFunction = keyFunction;
        }

        public TKey Add(TItem item)
        {
            var newKey = keyFunction(item);
            Store.Add(newKey, item);

            return newKey;
        }

        public Identified<TKey, TItem> GetById(TKey id)
        {
            return Store.ContainsKey(id)
                ? IdentifiedTools.Identified(id, Store[id])
                : null;
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