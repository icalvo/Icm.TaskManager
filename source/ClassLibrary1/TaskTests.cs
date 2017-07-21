using System;
using FluentAssertions;
using Icm.TaskManager.Domain.Chores;
using NodaTime;
using Xunit;

namespace Icm.ChoreManager.Tests
{
    public class TaskTests
    {
        [Fact]
        public void Create()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            task.Description.Should().Be("My description");
            task.Priority.Should().Be(3);
            task.IsDone.Should().BeFalse();
        }

        [Fact]
        public void StartDate()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            var startDate = CreateInstant(2016, 1, 2);
            task.StartDate = startDate;

            task.StartDate.Should().Be(startDate);
        }

        [Fact]
        public void FinishDate()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            var finishDate = CreateInstant(2016, 1, 2);
            task.FinishDate = finishDate;

            task.FinishDate.Should().Be(finishDate);
        }

        [Fact]
        public void StartDate_WhenAfterFinishDate_ThrowsException()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            var finishDate = CreateInstant(2016, 1, 3);
            task.FinishDate = finishDate;

            var startDate = CreateInstant(2016, 1, 5);
            Action action = () => task.StartDate = startDate;

            action.ShouldThrow<Exception>();
        }

        private static Chore CreateTask(Instant now)
        {
            var task = Chore.Create(
                "My description",
                now.Plus(Duration.FromDays(2)),
                now);
            return task;
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}
