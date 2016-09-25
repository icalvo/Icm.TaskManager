using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public TItem GetById(TKey id)
        {
            if (Store.ContainsKey(id))
            {
                return Store[id];
            }

            return default(TItem);
        }

        public void Update(TKey key, TItem item)
        {
            Store[key] = item;
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