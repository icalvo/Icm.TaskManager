using FluentAssertions;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace Icm.ChoreManager.Tests
{
    public class ChoreServiceTests
    {
        private readonly FakeClock clock;

        public ChoreServiceTests()
        {
            clock = new FakeClock(CreateInstant(2016, 1, 1), Duration.FromDays(1));
        }

        [Fact]
        public void Create()
        {
            var sut = new ChoreService(clock);
            var chore = CreateChore(sut);

            chore.CreationDate.Should().Be(CreateInstant(2016, 1, 1));
            chore.Description.Should().Be("My description");
            chore.Priority.Should().Be(3);
            chore.IsDone.Should().BeFalse();
        }

        [Fact]
        public void Finish()
        {
            var sut = new ChoreService(clock);
            var chore = CreateChore(sut);

            sut.Finish(chore);
            var finishDate = CreateInstant(2016, 1, 2);
            chore.FinishDate.Should().Be(finishDate);
        }

        private static Chore CreateChore(IChoreService sut)
        {
            return sut.CreateChore(
                "My description",
                CreateInstant(2016, 1, 10));
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}