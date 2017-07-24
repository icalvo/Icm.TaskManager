using Icm.ChoreManager.Domain;
using NodaTime;

namespace Icm.ChoreManager.Application
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

        public override string ToString()
        {
            return $"{Kind} {Time}";
        }
    }
}