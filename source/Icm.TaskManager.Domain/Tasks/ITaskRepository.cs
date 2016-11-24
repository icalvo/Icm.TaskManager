using System.Collections.Generic;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface ITaskRepository : IRepository<TaskId, Task>
    {
        IEnumerable<Instant> GetActiveReminders();
    }
}
