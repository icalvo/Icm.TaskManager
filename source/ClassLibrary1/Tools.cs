using NodaTime;

namespace Icm.ChoreManager.Tests
{
    public static class Tools
    {
        public static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}