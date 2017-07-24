using System;
using System.Threading.Tasks;

namespace Icm.ChoreManager.Domain
{
    public interface IRepository<TKey, TItem> : IDisposable
    {
        Task<TKey> AddAsync(TItem item);

        Task<Identified<TKey, TItem>> GetByIdAsync(TKey id);

        Task UpdateAsync(Identified<TKey, TItem> identifiedChore);

        Task DeleteAsync(TKey key);

        Task SaveAsync();
    }
}