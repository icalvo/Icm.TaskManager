using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface IChoreRepository : IRepository<ChoreId, Chore>
    {
        Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveReminders();
    }
}
