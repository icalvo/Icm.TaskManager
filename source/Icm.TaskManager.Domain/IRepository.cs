using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface IRepository<TKey, TItem>
    {
        TKey Add(TItem item);

        Identified<TKey, TItem> GetById(TKey id);

        void Update(Identified<TKey, TItem> value);

        void Delete(TKey key);

        void Save();
    }
}