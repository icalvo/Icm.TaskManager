namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface IRepository<TKey, TItem>
    {
        TKey Add(TItem item);

        TItem GetById(TKey id);

        void Update(TKey key, TItem item);

        void Delete(TKey key);

        void Save();
    }
}