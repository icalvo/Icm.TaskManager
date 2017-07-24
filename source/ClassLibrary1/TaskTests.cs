using System;
using FluentAssertions;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;
using Xunit;

namespace Icm.ChoreManager.Tests
{
    public class TaskTests
    {
        [Fact]
        public void Create()
        {
            var chore = CreateChore(CreateInstant(2016, 1, 1));

            chore.Description.Should().Be("My description");
            chore.Priority.Should().Be(3);
            chore.IsDone.Should().BeFalse();
        }

        [Fact]
        public void StartDate()
        {
            var chore = CreateChore(CreateInstant(2016, 1, 1));

            var startDate = CreateInstant(2016, 1, 2);
            chore.StartDate = startDate;

            chore.StartDate.Should().Be(startDate);
        }

        [Fact]
        public void FinishDate()
        {
            var chore = CreateChore(CreateInstant(2016, 1, 1));

            var finishDate = CreateInstant(2016, 1, 2);
            chore.FinishDate = finishDate;

            chore.FinishDate.Should().Be(finishDate);
        }

        [Fact]
        public void StartDate_WhenAfterFinishDate_ThrowsException()
        {
            var chore = CreateChore(CreateInstant(2016, 1, 1));

            var finishDate = CreateInstant(2016, 1, 3);
            chore.FinishDate = finishDate;

            var startDate = CreateInstant(2016, 1, 5);
            Action action = () => chore.StartDate = startDate;

            action.ShouldThrow<Exception>();
        }

        private static Chore CreateChore(Instant now)
        {
            var chore = Chore.Create(
                "My description",
                now.Plus(Duration.FromDays(2)),
                now);
            return chore;
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}
