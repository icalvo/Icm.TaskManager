using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using NodaTime;

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
}