using System.Collections.Generic;
using System.Threading.Tasks;
using Icm.ChoreManager.Domain;
using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public interface IChoreRepository : IRepository<ChoreId, Chore>
    {
        Task<IEnumerable<(Instant Time, TimeKind Kind)>> GetActiveRemindersAsync();
    }
}
