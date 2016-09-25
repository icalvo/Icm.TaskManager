using System;
using FluentAssertions;
using Icm.TaskManager.Domain.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace Icm.TaskManager.Domain.Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod]
        public void Create()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            task.Description.Should().Be("My description");
            task.Priority.Should().Be(3);
            task.Notes.Should().Be("Notes");
            task.IsDone.Should().BeFalse();
        }

        [TestMethod]
        public void StartDate()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            var startDate = CreateInstant(2016, 1, 2);
            task.StartDate = startDate;

            task.StartDate.Should().Be(startDate);
        }

        [TestMethod]
        public void FinishDate()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            var finishDate = CreateInstant(2016, 1, 2);
            task.FinishDate = finishDate;

            task.FinishDate.Should().Be(finishDate);
        }

        [TestMethod]
        public void StartDate_WhenAfterFinishDate_ThrowsException()
        {
            var task = CreateTask(CreateInstant(2016, 1, 1));

            var finishDate = CreateInstant(2016, 1, 3);
            task.FinishDate = finishDate;

            var startDate = CreateInstant(2016, 1, 5);
            Action action = () => task.StartDate = startDate;

            action.ShouldThrow<Exception>();
        }

        private static Task CreateTask(Instant now)
        {
            var task = Task.Create(
                "My description",
                null,
                now.Plus(Duration.FromStandardDays(2)),
                null,
                null,
                3,
                "Notes",
                "labels",
                now);
            return task;
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}
