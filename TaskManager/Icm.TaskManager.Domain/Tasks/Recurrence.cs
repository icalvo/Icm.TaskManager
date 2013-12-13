using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain.Tasks
{
    public abstract class Recurrence
    {
        public int Id { get; set; }
        public TimeSpan RepeatInterval { get; internal set; }

        public abstract Task CreateRecurringTask(Task task, ICurrentDateProvider currentDateProvider);
    }

    public class DueDateRecurrence : Recurrence
    {

        public override Task CreateRecurringTask(Task task, ICurrentDateProvider currentDateProvider)
        {
            DateTime dueDate = task.DueDate.Add(task.Recurrence.RepeatInterval);
            Task newTask = task.CopyWithNewDueDate(dueDate, currentDateProvider);
            return newTask;
        }
    }

    public class FinishDateRecurrence : Recurrence
    {

        public override Task CreateRecurringTask(Task task, ICurrentDateProvider currentDateProvider)
        {
            Task newTask = null;
            if (task.FinishDate.HasValue)
            {
                DateTime dueDate = task.FinishDate.Value.Add(task.Recurrence.RepeatInterval);
                newTask = task.CopyWithNewDueDate(dueDate, currentDateProvider);
            }
            return newTask;
        }
    }
}
