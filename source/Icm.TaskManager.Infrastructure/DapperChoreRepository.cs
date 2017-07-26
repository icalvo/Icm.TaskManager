using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Icm.ChoreManager.Domain;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;

namespace Icm.ChoreManager.Infrastructure
{
    public static class Ext2
    {
        public static Task<T> QuerySingleAsync<T>(
            this IDbTransaction tran, string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return tran.Connection.QuerySingleAsync<T>(sql, param, tran, commandTimeout, commandType);
        }

        public static Task<T> QuerySingleOrDefaultAsync<T>(
            this IDbTransaction tran, string sql,
            object param = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return tran.Connection.QuerySingleOrDefaultAsync<T>(sql, param, tran, commandTimeout, commandType);
        }

        public static Task<int> ExecuteAsync(this IDbTransaction transaction, string sql, object param = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            return transaction.Connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }

    public class DapperChoreRepository : IChoreRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;

        public DapperChoreRepository(IDbConnection connection)
        {
            this.connection = connection;
            transaction = this.connection.BeginTransaction();
        }

        public async Task<ChoreId> AddAsync(Chore item)
        {
            var id = await transaction.QuerySingleAsync<int>("INSERT INTO Tasks () VALUES ()");
            return id;
        }

        public async Task<Identified<ChoreId, Chore>> GetByIdAsync(ChoreId id)
        {
            var row = await transaction.QuerySingleOrDefaultAsync<ChoreMemento>("SELECT * FROM Tasks", id.Value);

            return Identified.Create((ChoreId)row.Id, Chore.FromMemento(row));
        }

        public async Task UpdateAsync(Identified<ChoreId, Chore> identifiedChore)
        {
            await transaction.ExecuteAsync("UPDATE Tasks SET ");
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(ChoreId key)
        {
            await transaction.ExecuteAsync("DELETE FROM Tasks WHERE Id=@Id", key.Value);
        }

        public Task SaveAsync()
        {
            transaction.Commit();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveRemindersAsync()
        {
            throw new NotImplementedException();
        }


        private bool disposed;
        public void Dispose()
        {
            if (disposed) return;

            transaction.Rollback();
            connection?.Dispose();
            transaction?.Dispose();
            disposed = true;
        }
    }
}
