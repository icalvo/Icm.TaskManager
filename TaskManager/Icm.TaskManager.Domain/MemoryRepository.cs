using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            this.store = initialElements.ToDictionary(item => keyFunction(item));
            this.keyFunction = keyFunction;
        }

        public void Create(TItem item)
        {
            store.Add(keyFunction(item), item);
        }

        public TItem GetById(TKey id)
        {
            return store[id];
        }

        public bool Update(TItem item)
        {
            TKey key = keyFunction(item);
            if (store.ContainsKey(key)) {
                store[key] = item;
                return true;
            }
            else {
                return false;
            }
        }

        public void Delete(TItem item)
        {
            store.Remove(keyFunction(item));
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
