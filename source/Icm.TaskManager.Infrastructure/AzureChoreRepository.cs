using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.ChoreManager.Domain;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Icm.ChoreManager.Infrastructure
{
    public class AzureChoreRepository : IChoreRepository {

        private readonly TableBatchOperation tableBatchOperation;
        private readonly CloudTable table;


        public AzureChoreRepository(string connectionString = "UseDevelopmentStorage=true")
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            table = tableClient.GetTableReference("chores");

            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync().Wait();

            tableBatchOperation = new TableBatchOperation();
        }

        public Task<ChoreId> AddAsync(Chore item)
        {
            var choreEntity = new ChoreEntity(item);
            tableBatchOperation.Insert(choreEntity);
            return Task.FromResult<ChoreId>(choreEntity.Id);
        }

        public async Task<Identified<ChoreId, Chore>> GetByIdAsync(ChoreId id)
        {
            var query = new TableQuery<ChoreEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.Value.ToString()));
            var results = await table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
            var result = results.Single();

            return new Identified<ChoreId, Chore>(result.Id, Chore.FromMemento(result.ToMemento(result.Id)));
        }

        public Task UpdateAsync(Identified<ChoreId, Chore> identifiedChore)
        {
            tableBatchOperation.Replace(new ChoreEntity(identifiedChore.Id, identifiedChore.Value));
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ChoreId key)
        {
            tableBatchOperation.Delete(new TableEntity(key.Value.ToString(), key.Value.ToString()));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveRemindersAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            return table.ExecuteBatchAsync(tableBatchOperation);
        }

        public void Dispose()
        {
        }
    }
}