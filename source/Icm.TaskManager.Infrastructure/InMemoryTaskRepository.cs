using System;
using System.Collections.Generic;
using System.Linq;
using Icm.TaskManager.Domain;
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

        public IEnumerable<Tuple<Instant, TimeKind>> GetActiveReminders()
        {
            return Storage.Values
                .SelectMany(GetActiveTimes);
        }

        private static IEnumerable<Tuple<Instant, TimeKind>> GetActiveTimes(TaskMemento task)
        {
            if (task.StartDate.HasValue)
            {
                yield return Tuple.Create(task.StartDate.Value, TimeKind.StartDate);
            }

            yield return Tuple.Create(task.DueDate, TimeKind.DueDate);

            if (task.FinishDate.HasValue)
            {
                yield return Tuple.Create(task.FinishDate.Value, TimeKind.FinishDate);
            }

            foreach (var reminder in task.Reminders)
            {
                yield return Tuple.Create(reminder, TimeKind.Reminder);
            }
        }
    }
}