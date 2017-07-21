using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icm.TaskManager.Domain;
using Icm.TaskManager.Domain.Chores;
using NodaTime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NodaTime.Text;

namespace Icm.TaskManager.Infrastructure
{
    public class ChoreEntity : TableEntity
    {
        public ChoreEntity(Chore chore)
        {
            var pattern = InstantPattern.General;
            Id = Guid.NewGuid();
            Description = chore.Description;
            CreationDate = pattern.Format(chore.CreationDate);
            StartDate = chore.StartDate.HasValue? pattern.Format(chore.StartDate.Value) : string.Empty;
            DueDate = pattern.Format(chore.DueDate);
            FinishDate = chore.FinishDate.HasValue ? pattern.Format(chore.FinishDate.Value) : string.Empty;
            Priority = chore.Priority;
            Recurrence = chore.Recurrence;
            Notes = chore.Notes;
            Labels = chore.Labels;
            Reminders = chore.Reminders;
        }

        public ChoreEntity(ChoreId id, Chore chore) : this(chore)
        {
            Id = id;
        }

        public ChoreEntity() { }

        private Guid id;

        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                PartitionKey = id.ToString();
                RowKey = id.ToString();
            }
        }

        public string Description { get; set; }

        public string CreationDate { get; set; }

        public string StartDate { get; set; }

        public string DueDate { get; set; }

        public string FinishDate { get; set; }

        public Recurrence Recurrence { get; set; }

        public int Priority { get; set; }

        public string Notes { get; set; }

        public string Labels { get; set; }

        public ICollection<Instant> Reminders { get; set; }


        public ChoreMemento ToMemento(ChoreId id)
        {
            return new ChoreMemento
            {
                Id = id.Value,
                CreationDate = Parse(CreationDate).Value,
                Description = Description,
                DueDate = Parse(DueDate).Value,
                FinishDate = Parse(FinishDate),
                Labels = Labels,
                Notes = Notes,
                Recurrence = Recurrence,
                Reminders = Reminders == null ? new List<Instant>() : new List<Instant>(Reminders),
                Priority = Priority,
                StartDate = Parse(StartDate)
            };
        }

        private static Instant? Parse(string instant)
        {
            var result = InstantPattern.General.Parse(instant);

            if (result.Success)
            {
                return result.Value;
            }

            return null;
        }
    }


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

        public Task<ChoreId> Add(Chore item)
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

        public Task Update(Identified<ChoreId, Chore> identifiedChore)
        {
            tableBatchOperation.Replace(new ChoreEntity(identifiedChore.Id, identifiedChore.Value));
            return Task.CompletedTask;
        }

        public Task Delete(ChoreId key)
        {
            tableBatchOperation.Delete(new TableEntity(key.Value.ToString(), key.Value.ToString()));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveReminders()
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            return table.ExecuteBatchAsync(tableBatchOperation);
        }

        public void Dispose()
        {
        }
    }
}