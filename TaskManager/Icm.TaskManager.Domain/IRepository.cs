namespace Icm.TaskManager.Domain
{
    using System.Collections.Generic;

    public interface IRepository<TKey, TItem> : IEnumerable<TItem>
    {
        void Create(TItem item);

        TItem GetById(TKey id);

        bool Update(TItem item);

        void Delete(TItem item);
    }
}