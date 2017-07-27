using System.Threading.Tasks;
using FluentAssertions;
using Icm.ChoreManager.Application;
using Icm.ChoreManager.Domain.Chores;
using Icm.ChoreManager.Infrastructure;
using NodaTime;
using NodaTime.Testing;
using Xunit;
using static Icm.ChoreManager.Tests.Tools;

namespace Icm.ChoreManager.Tests
{
    public class ChoreApplicationServiceTests
    {
        [Fact]
        public async Task GivenChoreWithDueDateRecurrence_WhenFinished_FinishDateEstablished()
        {
            var repo = InMemoryChoreRepository.WithInstanceStorage();
            var clock = new FakeClock(CreateInstant(2016, 5, 6));
            IChoreApplicationService sut = new ChoreApplicationService(() => repo, clock);

            var choreId = await sut.CreateAsync(
                "My description",
                CreateInstant(2016, 1, 10));

            await sut.SetRecurrenceToDueDateAsync(choreId, Duration.FromDays(2));

            clock.AdvanceDays(1);

            var secondChoreId = await sut.FinishAsync(choreId);

            secondChoreId.Should().NotBeNull();
            repo.Should().HaveCount(2);

            var chore = (await repo.GetByIdAsync(new ChoreId(choreId))).Value;
            chore.IsDone.Should().BeTrue();
            chore.FinishDate.Should().Be(CreateInstant(2016, 5, 7));
        }

        [Fact]
        public async Task GivenChoreWithDueDateRecurrence_WhenFinished_DueDateRecurrentTaskAdded()
        {
            var repo = InMemoryChoreRepository.WithInstanceStorage();
            var clock = new FakeClock(CreateInstant(2016, 5, 6), Duration.FromDays(1));
            IChoreApplicationService sut = new ChoreApplicationService(() => repo, clock);

            var choreId = await sut.CreateAsync(
                "My description",
                CreateInstant(2016, 1, 10));

            await sut.SetRecurrenceToDueDateAsync(choreId, Duration.FromDays(3));

            var secondChoreId = await sut.FinishAsync(choreId);

            secondChoreId.Should().NotBeNull();
            repo.Should().HaveCount(2);

            var chore = (await repo.GetByIdAsync(new ChoreId(choreId))).Value;
            chore.IsDone.Should().BeTrue();

            var recurringChore = (await repo.GetByIdAsync(new ChoreId(secondChoreId.Value))).Value;
            recurringChore.IsDone.Should().BeFalse();
            recurringChore.DueDate = chore.DueDate + Duration.FromDays(3);
        }
    }
}