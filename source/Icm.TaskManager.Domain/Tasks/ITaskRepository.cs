using System.Collections.Generic;
using Icm.TaskManager.Infrastructure.Interfaces;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface ITaskRepository : IRepository<TaskId, Task>
    {
        IEnumerable<Instant> GetActiveReminders();
    }
}
