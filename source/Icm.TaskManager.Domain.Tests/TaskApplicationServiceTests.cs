using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tests.Fakes;
using Icm.TaskManager.Infrastructure;
using Icm.TaskManager.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using NodaTime.Testing;

namespace Icm.TaskManager.Domain.Tests
{
    [TestClass]
    public class TaskApplicationServiceTests
    {
        [TestMethod]
        public void GivenTaskWithDueDateRecurrence_WhenFinished_FinishDateEstablished()
        {
            var repo = new FakeTaskRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6));
            var sut = new TaskApplicationService(repo, clock);

            var taskId = sut.CreateDueDateRecurringTask(
                "My description",
                CreateInstant(2016, 1, 10),
                Duration.FromStandardDays(2),
                3,
                "Notes",
                "labels");

            clock.AdvanceDays(1);

            var secondTaskId = sut.FinishTask(taskId);

            secondTaskId.Should().HaveValue();
            repo.Should().HaveCount(2);

            var task = repo.GetById(new TaskId(taskId));
            task.IsDone.Should().BeTrue();
            task.FinishDate.Should().Be(CreateInstant(2016, 5, 7));
        }

        [TestMethod]
        public void GivenTaskWithDueDateRecurrence_WhenFinished_DueDateRecurrentTaskAdded()
        {
            var repo = new FakeTaskRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6), Duration.FromStandardDays(1));
            var sut = new TaskApplicationService(repo, clock);

            var taskId = sut.CreateDueDateRecurringTask(
                "My description",
                CreateInstant(2016, 1, 10),
                Duration.FromStandardDays(3),
                3,
                "Notes",
                "labels");

            var secondTaskId = sut.FinishTask(taskId);

            secondTaskId.Should().HaveValue();
            repo.Should().HaveCount(2);

            var task = repo.GetById(new TaskId(taskId));
            task.IsDone.Should().BeTrue();

            var recurringTask = repo.GetById(new TaskId(secondTaskId.Value));
            recurringTask.IsDone.Should().BeFalse();
            recurringTask.DueDate = task.DueDate + Duration.FromStandardDays(3);
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}