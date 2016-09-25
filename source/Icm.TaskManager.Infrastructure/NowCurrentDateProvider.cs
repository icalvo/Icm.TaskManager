using Icm.TaskManager.Domain;
using Icm.TaskManager.Infrastructure.Interfaces;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class NowCurrentDateProvider : ICurrentDateProvider
    {
        public Instant Now => SystemClock.Instance.Now;
    }
}
