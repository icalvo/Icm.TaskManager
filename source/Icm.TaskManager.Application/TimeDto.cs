using Icm.TaskManager.Domain;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class TimeDto
    {
        public TimeDto(Instant time, TimeKind kind)
        {
            Time = time;
            Kind = kind;
        }

        public Instant Time { get; }

        public TimeKind Kind { get; }
    }
}