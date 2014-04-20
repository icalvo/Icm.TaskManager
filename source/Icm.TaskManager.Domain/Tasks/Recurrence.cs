using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public abstract class Recurrence
    {
        public int Id { get; set; }

        public Duration RepeatInterval { get; internal set; }

        public abstract Task CreateRecurringTask(Task task, ICurrentDateProvider currentDateProvider);
    }
}
