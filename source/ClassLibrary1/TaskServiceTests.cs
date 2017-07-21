using FluentAssertions;
using Icm.TaskManager.Domain.Chores;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace Icm.ChoreManager.Tests
{
    public class TaskServiceTests
    {
        private readonly FakeClock clock;

        public TaskServiceTests()
        {
            clock = new FakeClock(CreateInstant(2016, 1, 1), Duration.FromDays(1));
        }

        [Fact]
        public void Create()
        {
            var sut = new ChoreService(clock);
            var task = CreateTask(sut);

            task.CreationDate.Should().Be(CreateInstant(2016, 1, 1));
            task.Description.Should().Be("My description");
            task.Priority.Should().Be(3);
            task.IsDone.Should().BeFalse();
        }

        [Fact]
        public void Finish()
        {
            var sut = new ChoreService(clock);
            var task = CreateTask(sut);

            sut.Finish(task);
            var finishDate = CreateInstant(2016, 1, 2);
            task.FinishDate.Should().Be(finishDate);
        }

        private static Chore CreateTask(IChoreService sut)
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