using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain
{
    public interface ITaskRepository : IEnumerable<Task>
    {
        void Create(Task task);
        Task GetById(int id);
    }
}
