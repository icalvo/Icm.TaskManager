using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Icm.TaskManager.Domain;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tasks.Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class DapperChoreRepository : IChoreRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;

        public DapperChoreRepository(IDbConnection connection)
        {
            this.connection = connection;
            transaction = this.connection.BeginTransaction();
        }


        public async Task<ChoreId> Add(Chore item)
        {
            var id = await connection.QuerySingleAsync<int>("INSERT INTO Tasks () VALUES ()");
            return id;
        }

        public async Task<Identified<ChoreId, Chore>> GetByIdAsync(ChoreId id)
        {
            var row = await connection.QuerySingleOrDefaultAsync<ChoreMemento>("SELECT * FROM Tasks", id.Value, transaction);

            return Identified.Create((ChoreId)row.Id, Chore.FromMemento(row));
        }

        public async Task Update(Identified<ChoreId, Chore> identifiedChore)
        {
            await connection.ExecuteAsync("UPDATE Tasks SET ");
            throw new NotImplementedException();
        }

        public async Task Delete(ChoreId key)
        {
            await connection.ExecuteAsync("DELETE FROM Tasks WHERE Id=@Id", key.Value);
        }

        public Task Save()
        {
            transaction.Commit();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}
