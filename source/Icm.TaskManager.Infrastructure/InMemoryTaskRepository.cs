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

        public Identified<TaskId, Task> GetById(TaskId id)
        {
            return IdentifiedTools.Identified(id, Task.FromMemento(Storage[id]));
        }

        public void Update(Identified<TaskId, Task> value)
        {
            Storage[value.Id] = value.Value.Save();
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