using System.Threading.Tasks;
using FluentAssertions;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Domain.Tests.Fakes;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace Icm.TaskManager.Tests
{
    public class ChoreApplicationServiceTests
    {
        [Fact]
        public async Task GivenChoreWithDueDateRecurrence_WhenFinished_FinishDateEstablished()
        {
            var repo = new FakeChoreRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6));
            IChoreApplicationService sut = new ChoreApplicationService(repo, clock);

            var choreId = await sut.Create(
                "My description",
                CreateInstant(2016, 1, 10));

            await sut.ChangeRecurrenceToDueDate(choreId, Duration.FromDays(2));

            clock.AdvanceDays(1);

            var secondChoreId = await sut.Finish(choreId);

            secondChoreId.Should().HaveValue();
            repo.Should().HaveCount(2);

            var chore = (await repo.GetByIdAsync(new ChoreId(choreId))).Value;
            chore.IsDone.Should().BeTrue();
            chore.FinishDate.Should().Be(CreateInstant(2016, 5, 7));
        }

        [Fact]
        public async Task GivenChoreWithDueDateRecurrence_WhenFinished_DueDateRecurrentTaskAdded()
        {
            var repo = new FakeChoreRepository();
            var clock = new FakeClock(CreateInstant(2016, 5, 6), Duration.FromDays(1));
            IChoreApplicationService sut = new ChoreApplicationService(repo, clock);

            var choreId = await sut.Create(
                "My description",
                CreateInstant(2016, 1, 10));

            await sut.ChangeRecurrenceToDueDate(choreId, Duration.FromDays(3));

            var secondChoreId = await sut.Finish(choreId);

            secondChoreId.Should().HaveValue();
            repo.Should().HaveCount(2);

            var chore = (await repo.GetByIdAsync(new ChoreId(choreId))).Value;
            chore.IsDone.Should().BeTrue();

            var recurringChore = (await repo.GetByIdAsync(new ChoreId(secondChoreId.Value))).Value;
            recurringChore.IsDone.Should().BeFalse();
            recurringChore.DueDate = chore.DueDate + Duration.FromDays(3);
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}