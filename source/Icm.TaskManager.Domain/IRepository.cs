using System.Threading.Tasks;
using Icm.TaskManager.Domain.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Icm.TaskManager.Domain
{
    public interface IRepository<TKey, TItem>
    {
        Task<TKey> Add(TItem item);

        Task<Identified<TKey, TItem>> GetByIdAsync(TKey id);

        Task Update(Identified<TKey, TItem> identifiedChore);

        Task Delete(TKey key);

        Task Save();
    }
}