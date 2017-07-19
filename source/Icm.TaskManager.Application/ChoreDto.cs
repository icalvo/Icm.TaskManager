using Icm.TaskManager.Domain;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class ChoreDto
    {
        public ChoreDto(Instant time, TimeKind kind)
        {
            Time = time;
            Kind = kind;
        }

        public Instant Time { get; }

        public TimeKind Kind { get; }

        public override string ToString()
        {
            return $"{Kind} {Time}";
        }
    }
}