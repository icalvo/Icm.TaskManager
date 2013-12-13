using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface ITaskService
    {
        Task CreateTask(string description, System.DateTime? startDate, System.DateTime dueDate, string recurrenceType, TimeSpan? repeatInterval, int priority, string notes, string labels);

        Task Finish(Task task);
    }
}
