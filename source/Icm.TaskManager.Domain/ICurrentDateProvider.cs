namespace Icm.TaskManager.Domain
{
    using NodaTime;

    public interface ICurrentDateProvider
    {
        Instant Now { get; }
    }
}