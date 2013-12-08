using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain
{
    public interface IRepository<TKey, TItem> : IEnumerable<TItem>
    {
        void Create(TItem item);
        TItem GetById(TKey id);
        bool Update(TItem item);
        void Delete(TItem item);
    }
}
