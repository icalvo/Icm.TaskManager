using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Domain.Tests.Fakes
{
    public class FakeChoreRepository : MemoryRepoKeyRepository<ChoreId, Chore>, IChoreRepository
    {
        public FakeChoreRepository()
            : base(lastKey => new ChoreId(lastKey + 1))
        {
        }

        public Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}
