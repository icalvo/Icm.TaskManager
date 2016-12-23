using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Icm.TaskManager.Domain;
using Icm.TaskManager.Domain.Tasks;
using Task = Icm.TaskManager.Domain.Tasks.Task;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
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

        public Identified<TaskId, Task> GetById(TaskId id)
        {
            var row = connection.QuerySingleOrDefault("SELECT * FROM Tasks", id.Value, transaction);

            throw new NotImplementedException();
        }

        public void Update(Identified<TaskId, Task> value)
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

        public IEnumerable<(Instant Time, TimeKind Kind)> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}
