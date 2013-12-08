using System.Collections.Generic;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface ITaskRepository : IRepository<int, Task>
    {
        IEnumerable<Reminder> GetActiveReminders();
    }
}
