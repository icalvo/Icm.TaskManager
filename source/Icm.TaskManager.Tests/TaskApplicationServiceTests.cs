using FluentAssertions;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tests.Fakes;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace Icm.TaskManager.Tests
{
    public class TaskApplicationServiceTests
    {
        [Fact]
        public void GivenTaskWithDueDateRecurrence_WhenFinished_FinishDateEstablished()
        {
            var repo = new FakeTaskRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6));
            ITaskApplicationService sut = new TaskApplicationService(repo, clock);

            var taskId = sut.CreateTask(
                "My description",
                CreateInstant(2016, 1, 10));

            sut.ChangeRecurrenceToDueDate(taskId, Duration.FromDays(2));

            clock.AdvanceDays(1);

            var secondTaskId = sut.FinishTask(taskId);

            secondTaskId.Should().HaveValue();
            repo.Should().HaveCount(2);

            var task = repo.GetById(new TaskId(taskId)).Value;
            task.IsDone.Should().BeTrue();
            task.FinishDate.Should().Be(CreateInstant(2016, 5, 7));
        }

        [Fact]
        public void GivenTaskWithDueDateRecurrence_WhenFinished_DueDateRecurrentTaskAdded()
        {
            var repo = new FakeTaskRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6), Duration.FromDays(1));
            ITaskApplicationService sut = new TaskApplicationService(repo, clock);

            var taskId = sut.CreateTask(
                "My description",
                CreateInstant(2016, 1, 10));

            sut.ChangeRecurrenceToDueDate(taskId, Duration.FromDays(3));

            var secondTaskId = sut.FinishTask(taskId);

            secondTaskId.Should().HaveValue();
            repo.Should().HaveCount(2);

            var task = repo.GetById(new TaskId(taskId)).Value;
            task.IsDone.Should().BeTrue();

            var recurringTask = repo.GetById(new TaskId(secondTaskId.Value)).Value;
            recurringTask.IsDone.Should().BeFalse();
            recurringTask.DueDate = task.DueDate + Duration.FromDays(3);
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}