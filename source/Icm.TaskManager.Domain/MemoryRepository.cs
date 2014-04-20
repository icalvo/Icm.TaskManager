using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Icm.TaskManager.Domain
{
    public class MemoryRepository<TKey, TItem> : IRepository<TKey, TItem>
    {
        private readonly Dictionary<TKey, TItem> store;
        private readonly Func<TItem, TKey> keyFunction;

        public MemoryRepository(Func<TItem, TKey> keyFunction)
        {
            this.store = new Dictionary<TKey, TItem>();
            this.keyFunction = keyFunction;
        }

        public MemoryRepository(IEnumerable<TItem> initialElements, Func<TItem, TKey> keyFunction)
        {
            this.store = initialElements.ToDictionary(keyFunction);
            this.keyFunction = keyFunction;
        }

        public void Create(TItem item)
        {
            this.store.Add(this.keyFunction(item), item);
        }

        public TItem GetById(TKey id)
        {
            if (this.store.ContainsKey(id))
            {
                return this.store[id];
            }

            return default(TItem);
        }

        public bool Update(TItem item)
        {
            TKey key = this.keyFunction(item);
            if (this.store.ContainsKey(key))
            {
                this.store[key] = item;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Delete(TItem item)
        {
            this.store.Remove(this.keyFunction(item));
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return this.store.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.store.Values.GetEnumerator();
        }
    }
}