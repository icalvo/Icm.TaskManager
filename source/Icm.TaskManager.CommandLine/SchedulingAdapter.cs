using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Icm.ChoreManager.Application;
using Icm.ChoreManager.Domain;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;

namespace Icm.ChoreManager.CommandLine
{
    public class SchedulingAdapter : IChoreApplicationServiceSchedulingAdapter
    {
        private readonly IChoreApplicationService impl;
        private readonly IScheduler scheduler;
        private readonly IObserver<TimeDto> timerExpirations;
        private readonly IDictionary<Instant, List<IDisposable>> pending;

        public SchedulingAdapter(
            IChoreApplicationService impl,
            IScheduler scheduler,
            IObserver<TimeDto> timerExpirations)
        {
            this.impl = impl;
            this.scheduler = scheduler;
            this.timerExpirations = timerExpirations;
            pending = new Dictionary<Instant, List<IDisposable>>();
        }

        public void Complete()
        {
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
            if (!pending.ContainsKey(pendingTime.Time))
            {
                pending.Add(pendingTime.Time, new List<IDisposable>());
            }
            pending[pendingTime.Time].Add(scheduleHandler);
        }

        private void Unschedule(Instant instant)
        {
            pending[instant].ForEach(s => s.Dispose());
        }

        async Task IChoreApplicationService.AddReminderAsync(ChoreId choreId, Instant reminder)
        {
            await impl.AddReminderAsync(choreId, reminder);
            Schedule(new TimeDto(reminder, TimeKind.Reminder));
        }

        async Task IChoreApplicationService.ChangeDueDateAsync(ChoreId choreId, Instant newDueDate)
        {
            var dto = await impl.GetByIdAsync(choreId);
            await impl.ChangeDueDateAsync(choreId, newDueDate);
            Unschedule(dto.DueDate);
            Schedule(new TimeDto(newDueDate, TimeKind.DueDate));
        }

        async Task<ChoreId> IChoreApplicationService.CreateAsync(string description, Instant dueDate)
        {
            var choreId = await impl.CreateAsync(description, dueDate);
            var dto = await impl.GetByIdAsync(choreId);
            Schedule(new TimeDto(dto.DueDate, TimeKind.DueDate));
            return choreId;
        }


        Task IChoreApplicationService.SetRecurrenceToDueDateAsync(ChoreId choreId, Duration repeatInterval) => impl.SetRecurrenceToDueDateAsync(choreId, repeatInterval);

        Task IChoreApplicationService.ChangeStartDateAsync(ChoreId choreId, Instant newStartDate) => impl.ChangeStartDateAsync(choreId, newStartDate);

        Task IChoreApplicationService.SetRecurrenceToFinishDateAsync(ChoreId choreId, Duration repeatInterval) => impl.SetRecurrenceToFinishDateAsync(choreId, repeatInterval);

        Task IChoreApplicationService.RemoveRecurrenceAsync(ChoreId choreId) => impl.RemoveRecurrenceAsync(choreId);

        Task IChoreApplicationService.ChangeDescriptionAsync(ChoreId choreId, string newDescription) => impl.ChangeDescriptionAsync(choreId, newDescription);

        Task IChoreApplicationService.AddLabelsAsync(ChoreId choreId, string[] newLabels) => impl.AddLabelsAsync(choreId, newLabels);

        Task IChoreApplicationService.RemoveLabelsAsync(ChoreId choreId, string[] labelsToRemove) => impl.RemoveLabelsAsync(choreId, labelsToRemove);

        Task IChoreApplicationService.ChangeNotesAsync(ChoreId choreId, string newNotes) => impl.ChangeNotesAsync(choreId, newNotes);


        Task IChoreApplicationService.ChangePriorityAsync(ChoreId choreId, int newPriority) => impl.ChangePriorityAsync(choreId, newPriority);

        Task<ChoreMemento> IChoreApplicationService.GetByIdAsync(ChoreId choreId) => impl.GetByIdAsync(choreId);

        Task<IEnumerable<ChoreMemento>> IChoreApplicationService.GetPendingChoresAsync() => impl.GetPendingChoresAsync();

        Task<ChoreId?> IChoreApplicationService.FinishAsync(ChoreId choreId) => impl.FinishAsync(choreId);

        Task IChoreApplicationService.StartAsync(ChoreId choreId) => impl.StartAsync(choreId);

        Task<IEnumerable<TimeDto>> IChoreApplicationService.PendingTimesAsync() => impl.PendingTimesAsync();
    }
}
