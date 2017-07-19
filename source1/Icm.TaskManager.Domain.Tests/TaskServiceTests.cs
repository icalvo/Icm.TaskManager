using System;
using FluentAssertions;
using Icm.TaskManager.Domain.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using NodaTime.Testing;

namespace Icm.TaskManager.Domain.Tests
{
    [TestClass]
    public class TaskServiceTests
    {
        private readonly FakeClock clock;

        public TaskServiceTests()
        {
            clock = new FakeClock(CreateInstant(2016, 1, 1), Duration.FromDays(1));
        }

        [TestMethod]
        public void Create()
        {
            var sut = new TaskService(clock);
            var task = CreateTask(sut);

            task.CreationDate.Should().Be(CreateInstant(2016, 1, 1));
            task.Description.Should().Be("My description");
            task.Priority.Should().Be(3);
            task.IsDone.Should().BeFalse();
        }

        [TestMethod]
        public void Finish()
        {
            var sut = new TaskService(clock);
            var task = CreateTask(sut);

            sut.Finish(task);
            var finishDate = CreateInstant(2016, 1, 2);
            task.FinishDate.Should().Be(finishDate);
        }

        private static Task CreateTask(ITaskService sut)
        {
            return sut.CreateTask(
                "My description",
                CreateInstant(2016, 1, 10));
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}