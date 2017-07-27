using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using FluentAssertions;
using Icm.ChoreManager.Application;
using Icm.ChoreManager.CommandLine;
using Icm.ChoreManager.Infrastructure;
using NodaTime;
using NodaTime.Testing;
using Xunit;
using static Icm.ChoreManager.Tests.Tools;

namespace Icm.ChoreManager.Tests
{
    public class ScheduleAdapterTests
    {

        [Fact]
        public async Task DueDateAlertIsObserved()
        {
            var now = CreateInstant(2016, 1, 1);
            var choreDueDate = CreateInstant(2016, 1, 10);
            var afterChoreDueDate = CreateInstant(2016, 1, 11);

            var clock = new FakeClock(now, Duration.FromDays(1));

            var timerExpirations = new ListSubject<TimeDto>();
            var scheduler = new TestScheduler(now);

            var repo = InMemoryChoreRepository.WithInstanceStorage();

            IChoreApplicationService sut = new SchedulingAdapter(
                new ChoreApplicationService(() => repo, clock),
                scheduler, 
                timerExpirations);

            var choreId = await sut.CreateAsync(
                "My description",
                choreDueDate);

            timerExpirations.Should().HaveCount(0);

            scheduler.AdvanceTo(afterChoreDueDate);

            timerExpirations.Should().HaveCount(1);
        }

        private class ListSubject<T> : List<T>, IObserver<T>
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
    }
}