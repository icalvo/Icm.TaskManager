using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.Domain.Tests.Fakes
{
    public class FakeTaskRepository : MemoryRepository<int, Task>, ITaskRepository
    {
        public FakeTaskRepository() : base(task => task.Id)
        {
        }

        public FakeTaskRepository(IEnumerable<Task> items)
            : base(items, task => task.Id)
        {
        }

        public IEnumerable<Tasks.Reminder> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}
