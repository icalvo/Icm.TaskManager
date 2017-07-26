using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Icm.ChoreManager.Domain;
using Icm.ChoreManager.Domain.Chores;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using static System.Threading.Tasks.Task;

namespace Icm.ChoreManager.Infrastructure
{
    public class InMemoryChoreRepository : IChoreRepository, IEnumerable<ChoreMemento>
    {
        private const string Path = @"C:\Logs\store.json";
        private static readonly IDictionary<ChoreId, ChoreMemento> _staticStorage = new Dictionary<ChoreId, ChoreMemento>();
        private static int _latestKey;

        private static bool _staticStorageLoaded;

        private readonly IDictionary<ChoreId, ChoreMemento> storage;
        private readonly List<Operation> transactionLog = new List<Operation>();
        private static readonly JsonSerializer JsonSerializer;

        static InMemoryChoreRepository()
        {
            JsonSerializer = JsonSerializer.CreateDefault();
            JsonSerializer.Converters.Add(new ChoreIdConverter());
            JsonSerializer.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }
        private InMemoryChoreRepository(IDictionary<ChoreId, ChoreMemento> storage)
        {
            this.storage = storage;
        }

        private static IDictionary<ChoreId, ChoreMemento> StaticStorage
        {
            get
            {
                if (!_staticStorageLoaded)
                {
                    if (File.Exists(Path))
                    {
                        List<ChoreMemento> mementos;
                        using (StreamReader file = File.OpenText(Path))
                        {
                            using (var reader = new JsonTextReader(file))
                            {
                                mementos = 
                                    JsonSerializer.Deserialize<List<ChoreMemento>>(reader);
                            }
                        }
                        mementos.ForEach(m => _staticStorage.Add(m.Id, m));
                    }

                    if (_staticStorage.Any())
                    {
                        _latestKey = _staticStorage.Keys.Max(x => x.Value);
                    }
                    _staticStorageLoaded = true;
                }

                return _staticStorage;
            }
        }

        public static InMemoryChoreRepository WithStaticStorage()
        {
            return new InMemoryChoreRepository(StaticStorage);
        }


        public static InMemoryChoreRepository WithInstanceStorage()
        {
            return new InMemoryChoreRepository(new Dictionary<ChoreId, ChoreMemento>());
        }

        public Task<ChoreId> AddAsync(Chore item)
        {
            var newId = Interlocked.Increment(ref _latestKey);
            transactionLog.Add(new AddOperation(item.ToMemento(newId)));
            return FromResult<ChoreId>(newId);
        }

        public Task<Identified<ChoreId, Chore>> GetByIdAsync(ChoreId id)
        {
            return FromResult(Identified.Create(id, Chore.FromMemento(storage[id])));
        }

        public Task UpdateAsync(Identified<ChoreId, Chore> identifiedChore)
        {
            transactionLog.Add(new UpdateOperation(identifiedChore.Id, identifiedChore.ToMemento()));
            return CompletedTask;
        }

        public Task DeleteAsync(ChoreId key)
        {
            transactionLog.Add(new DeleteOperation(key));
            return CompletedTask;
        }

        public async Task SaveAsync()
        {
            foreach (var logEntry in transactionLog)
            {
                logEntry.Execute(storage);
            }

            if (_staticStorageLoaded)
            {
                await DumpToFileAsync();
            }

            transactionLog.Clear();
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveRemindersAsync()
        {
            return FromResult(
                storage.Values
                .SelectMany(GetActiveTimes));
        }

        public Task<IEnumerable<Identified<ChoreId, Chore>>> GetPendingAsync()
        {
            return FromResult(
                storage.Where(x => x.Value.FinishDate == null)
                    .Select(x => Identified.Create(x.Key, Chore.FromMemento(x.Value))));
        }

        private static IEnumerable<(Instant Time, TimeKind Kind)> GetActiveTimes(ChoreMemento chore)
        {
            yield return (chore.DueDate, TimeKind.DueDate);

            foreach (var reminder in chore.Reminders)
            {
                yield return (reminder, TimeKind.Reminder);
            }
        }

        public void Dispose()
        {
        }

        private static Task DumpToFileAsync()
        {
            using (StreamWriter file = new StreamWriter(File.Open(Path, FileMode.Create)))
            {
                using (var writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;

                    JsonSerializer.Serialize(writer, StaticStorage.Values);
                }
            }

            return CompletedTask;
        }

        public IEnumerator<ChoreMemento> GetEnumerator()
        {
            return storage.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private abstract class Operation
        {
            public abstract void Execute(IDictionary<ChoreId, ChoreMemento> storage);
        }

        private class AddOperation : Operation
        {
            private readonly ChoreMemento memento;

            public AddOperation(ChoreMemento memento)
            {
                this.memento = memento;
            }

            public override void Execute(IDictionary<ChoreId, ChoreMemento> storage)
            {
                storage[memento.Id] = memento;
            }
        }

        private class UpdateOperation : Operation
        {
            private readonly ChoreId id;
            private readonly ChoreMemento memento;

            public UpdateOperation(ChoreId id, ChoreMemento memento)
            {
                this.id = id;
                this.memento = memento;
            }
            public override void Execute(IDictionary<ChoreId, ChoreMemento> storage)
            {
                storage[id] = memento;
            }
        }


        private class DeleteOperation : Operation
        {
            private readonly ChoreId id;

            public DeleteOperation(ChoreId id)
            {
                this.id = id;
            }

            public override void Execute(IDictionary<ChoreId, ChoreMemento> storage)
            {
                storage.Remove(id);
            }
        }

        private class ChoreIdConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value.ToString());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var value = serializer.Deserialize<string>(reader);
                return ChoreId.Parse(value);
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ChoreId);
            }
        }
    }
}