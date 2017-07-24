using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Icm.ChoreManager.Application;
using Icm.ChoreManager.Domain;
using NodaTime;

namespace Icm.ChoreManager.CommandLine
{
    public class ChoreApplicationServiceSchedulingAdapter : IChoreApplicationService
    {
        private readonly IChoreApplicationService impl;
        private readonly IScheduler scheduler;
        private readonly IObserver<TimeDto> timerStarts;
        private readonly IObserver<TimeDto> timerExpirations;
        private readonly IDictionary<Instant, IDisposable> pending;

        public ChoreApplicationServiceSchedulingAdapter(
            IChoreApplicationService impl,
            IScheduler scheduler,
            IObserver<TimeDto> timerStarts,
            IObserver<TimeDto> timerExpirations)
        {
            this.impl = impl;
            this.scheduler = scheduler;
            this.timerStarts = timerStarts;
            this.timerExpirations = timerExpirations;
            pending = new Dictionary<Instant, IDisposable>();
        }

        public void Complete()
        {
            timerStarts.OnCompleted();
            timerExpirations.OnCompleted();
        }

        public async Task ScheduleExistingAsync()
        {

            var pendingTimes = await impl.PendingTimesAsync();
            pendingTimes.Execute(Schedule);
        }

        private void Schedule(TimeDto pendingTime)
        {
            var scheduleHandler = scheduler.Schedule(
                pendingTime.Time.ToDateTimeOffset(),
                () => timerExpirations.OnNext(pendingTime));
            pending.Add(pendingTime.Time, scheduleHandler);
            timerStarts.OnNext(pendingTime);
        }

        private void Unschedule(Instant instant)
        {
            var scheduleHandler = pending[instant];
            scheduleHandler.Dispose();
        }

        async Task IChoreApplicationService.AddReminderAsync(Guid choreId, Instant reminder)
        {
            await impl.AddReminderAsync(choreId, reminder);
            Schedule(new TimeDto(reminder, TimeKind.Reminder));
        }

        async Task IChoreApplicationService.ChangeDueDateAsync(Guid choreId, Instant newDueDate)
        {
            var dto = await impl.GetByIdAsync(choreId);
            await impl.ChangeDueDateAsync(choreId, newDueDate);
            Unschedule(dto.DueDate);
            Schedule(new TimeDto(newDueDate, TimeKind.DueDate));
        }

        async Task<Guid> IChoreApplicationService.CreateAsync(string description, Instant dueDate)
        {
            var choreId = await impl.CreateAsync(description, dueDate);
            var dto = await impl.GetByIdAsync(choreId);
            Schedule(new TimeDto(dto.DueDate, TimeKind.DueDate));
            return choreId;
        }


        Task IChoreApplicationService.ChangeRecurrenceToDueDateAsync(Guid choreId, Duration repeatInterval) => impl.ChangeRecurrenceToDueDateAsync(choreId, repeatInterval);

        Task IChoreApplicationService.ChangeStartDateAsync(Guid choreId, Instant newStartDate) => impl.ChangeStartDateAsync(choreId, newStartDate);

        Task IChoreApplicationService.ChangeRecurrenceToFinishDateAsync(Guid choreId, Duration repeatInterval) => impl.ChangeRecurrenceToFinishDateAsync(choreId, repeatInterval);

        Task IChoreApplicationService.ChangeDescriptionAsync(Guid choreId, string newDescription) => impl.ChangeDescriptionAsync(choreId, newDescription);

        Task IChoreApplicationService.ChangeLabelsAsync(Guid choreId, string newLabels) => impl.ChangeLabelsAsync(choreId, newLabels);

        Task IChoreApplicationService.ChangeNotesAsync(Guid choreId, string newNotes) => impl.ChangeNotesAsync(choreId, newNotes);


        Task IChoreApplicationService.ChangePriorityAsync(Guid choreId, int newPriority) => impl.ChangePriorityAsync(choreId, newPriority);

        Task<ChoreDto> IChoreApplicationService.GetByIdAsync(Guid choreId) => impl.GetByIdAsync(choreId);

        Task<IEnumerable<ChoreDto>> IChoreApplicationService.GetChoresFromAsync(Guid choreId) => impl.GetChoresFromAsync(choreId);

        Task<Guid?> IChoreApplicationService.FinishAsync(Guid choreId) => impl.FinishAsync(choreId);

        Task IChoreApplicationService.StartAsync(Guid choreId) => impl.StartAsync(choreId);

        Task<IEnumerable<TimeDto>> IChoreApplicationService.PendingTimesAsync() => impl.PendingTimesAsync();
    }
}
