using System.Collections.Generic;
using System.Threading;
using Edument.CQRS;
using FluentAssertions;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tests.Fakes;
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
            var eventBus = new MemoryEventBus();
            eventBus.Subscribe(new ReminderAddedEventHandler(clock, eventBus));
            var sut = new TaskApplicationService(repo, clock, eventBus, new MessageDispatcher(new InMemoryEventStore()));

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
            var eventBus = new MemoryEventBus();
            eventBus.Subscribe(new ReminderAddedEventHandler(clock, eventBus));
            var sut = new TaskApplicationService(repo, clock, eventBus, new MessageDispatcher(new InMemoryEventStore()));

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

        [TestMethod]
        public void GivenTaskWithReminder_ReminderFires()
        {
            var repo = new FakeTaskRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6));
            var eventBus = new MemoryEventBus();
            eventBus.Subscribe(new ReminderAddedEventHandler(clock, eventBus));

            var events = new List<object>();
            eventBus.Subscribe(
                ev => true,
                ev =>
                {
                    events.Add(ev);
                    return System.Threading.Tasks.Task.FromResult(false);
                });

            var sut = new TaskApplicationService(repo, clock, eventBus, new MessageDispatcher(new InMemoryEventStore()));

            var taskId = sut.CreateTask(
                "My description",
                CreateInstant(2016, 1, 10),
                3,
                "Notes",
                "labels");

            sut.AddTaskReminderRelativeToNow(taskId, Duration.FromMilliseconds(5));

            Thread.Sleep(15);

            events.Should().BeEquivalentTo(
                new ReminderAddedEvent(taskId, CreateInstant(2016, 5, 6).Plus(Duration.FromMilliseconds(5))),
                new ReminderElapsedEvent(taskId));
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}