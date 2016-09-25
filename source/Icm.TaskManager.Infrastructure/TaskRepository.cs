using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerContext context;

        public TaskRepository(TaskManagerContext context)
        {
            this.context = context;
        }

        public TaskId Add(Task task)
        {
            context.Tasks.Add(task);
            context.SaveChanges();

            return default(TaskId);
        }

        public void Update(TaskId key, Task task)
        {
            context.Entry(task).State = EntityState.Modified;
        }

        public void Delete(TaskId key)
        {
            context.Tasks.Remove(GetById(key));
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Task GetById(TaskId id)
        {
            return context.Tasks.Find(id);
        }

        public bool Exists(TaskId id)
        {
            return false;
            //// return context.Tasks.Any(task => Equals(task.Id, id));
        }

        public IEnumerable<Instant> GetActiveReminders()
        {
            var now = SystemClock.Instance.Now;
            return context.Tasks.SelectMany(task => task.Reminders).Where(reminder => reminder >= now);
        }
    }
}
