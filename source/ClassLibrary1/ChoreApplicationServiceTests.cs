using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using FluentAssertions;
using Icm.TaskManager.Application;
using Icm.TaskManager.CommandLine;
using Icm.TaskManager.Domain.Chores;
using Icm.TaskManager.Infrastructure;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace Icm.ChoreManager.Tests
{
    public class TestScheduler : VirtualTimeScheduler<Instant, Duration>
    {
        public TestScheduler(Instant initialTime) : base(initialTime, Comparer<Instant>.Default)
        {
        }

        protected override Instant Add(Instant absolute, Duration relative)
        {
            return absolute + relative;
        }

        protected override DateTimeOffset ToDateTimeOffset(Instant absolute)
        {
            return absolute.ToDateTimeOffset();
        }

        protected override Duration ToRelative(TimeSpan timeSpan)
        {
            return Duration.FromTimeSpan(timeSpan);
        }
    }

    public class ChoreApplicationServiceTests
    {
        [Fact]
        public async Task GivenChoreWithDueDateRecurrence_WhenFinished_FinishDateEstablished()
        {
            var repo = InMemoryChoreRepository.WithInstanceStorage();
            var clock = new FakeClock(CreateInstant(2016, 5, 6));
            IChoreApplicationService sut = new ChoreApplicationService(() => repo, clock);

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
            var repo = InMemoryChoreRepository.WithInstanceStorage();
            var clock = new FakeClock(CreateInstant(2016, 5, 6), Duration.FromDays(1));
            IChoreApplicationService sut = new ChoreApplicationService(() => repo, clock);

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

        private class  ListSubject<T> : List<T>, IObserver<T>
        {
            private readonly Subject<T> subject;

            public ListSubject()
            {
                subject = new Subject<T>();
                subject.Subscribe(Add);
            }
            public void OnCompleted() => subject.OnCompleted();

            public void OnError(Exception error) => subject.OnError(error);

            public void OnNext(T value) => subject.OnNext(value);
        }

        [Fact]
        public async Task bla()
        {
            var now = CreateInstant(2016, 1, 1);
            var choreDueDate = CreateInstant(2016, 1, 10);
            var afterChoreDueDate = CreateInstant(2016, 1, 11);

            var clock = new FakeClock(now, Duration.FromDays(1));

            var timerStarts = new ListSubject<TimeDto>();
            var timerExpirations = new ListSubject<TimeDto>();
            var scheduler = new TestScheduler(now);

            var repo = InMemoryChoreRepository.WithInstanceStorage();

            IChoreApplicationService sut = new SchedulingAdapter(
                new ChoreApplicationService(() => repo, clock),
                scheduler, 
                timerStarts, 
                timerExpirations);

            var choreId = await sut.Create(
                "My description",
                choreDueDate);

            timerStarts.Should().HaveCount(1);
            timerExpirations.Should().HaveCount(0);

            scheduler.AdvanceTo(afterChoreDueDate);
            timerStarts.Should().HaveCount(1);
            timerExpirations.Should().HaveCount(1);
        }

        private static Instant CreateInstant(int year, int month, int day)
        {
            return new LocalDate(year, month, day).AtMidnight().InUtc().ToInstant();
        }
    }
}