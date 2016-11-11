using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Icm.TaskManager.Domain.Tasks;
using Task = Icm.TaskManager.Domain.Tasks.Task;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private static readonly Dictionary<TaskId, Task> Storage = new Dictionary<TaskId, Task>();

        public TaskId Add(Task item)
        {
            var newId = new TaskId(Storage.Keys.Max(x => x.Value) + 1);
            Storage.Add(newId, item);

            return newId;
        }

        public Task GetById(TaskId id)
        {
            return Storage[id];
        }

        public void Update(TaskId key, Task item)
        {
            Storage[key] = item;
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

    public class DapperTaskRepository : ITaskRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;

        public DapperTaskRepository(IDbConnection connection)
        {
            this.connection = connection;
            transaction = this.connection.BeginTransaction();
        }


        public TaskId Add(Task item)
        {
            throw new NotImplementedException();
        }

        public Task GetById(TaskId id)
        {
            var row = connection.QuerySingleOrDefault("SELECT * FROM Tasks", id.Value, transaction);

            throw new NotImplementedException();
        }

        public void Update(TaskId key, Task item)
        {
            throw new NotImplementedException();
        }

        public void Delete(TaskId key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Instant> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}
