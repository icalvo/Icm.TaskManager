using System;
using System.Collections.Generic;
using System.Linq;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private static readonly Dictionary<TaskId, TaskMemento> Storage = new Dictionary<TaskId, TaskMemento>();

        public TaskId Add(Task item)
        {
            var newId = Storage.Keys.Any() ? Storage.Keys.Max(x => x.Value) + 1 : 1;
            Storage.Add(newId, item.Save());

            return newId;
        }

        public Task GetById(TaskId id)
        {
            return Task.FromMemento(Storage[id]);
        }

        public void Update(TaskId key, Task item)
        {
            Storage[key] = item.Save();
        }

        public void Delete(TaskId key)
        {
            Storage.Remove(key);
        }

        public void Save()
        {
        }

        public IEnumerable<Instant> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}