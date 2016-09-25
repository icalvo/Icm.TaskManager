using NodaTime;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface ICurrentDateProvider
    {
        Instant Now { get; }
    }
}