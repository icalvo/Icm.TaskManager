using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain;
using NodaTime;

namespace Icm.TaskManager.CommandLine
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

        public async Task ScheduleExisting()
        {
            var pendingTimes = await impl.PendingTimes();
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

        async Task IChoreApplicationService.AddReminder(Guid choreId, Instant reminder)
        {
            await impl.AddReminder(choreId, reminder);
            Schedule(new TimeDto(reminder, TimeKind.Reminder));
        }

        public async Task ChangeDueDate(Guid taskId, Instant newDueDate)
        {
            var dto = await impl.GetById(taskId);
            await impl.ChangeDueDate(taskId, newDueDate);
            Unschedule(dto.DueDate);
            Schedule(new TimeDto(newDueDate, TimeKind.DueDate));
        }

        async Task<Guid> IChoreApplicationService.Create(string description, Instant dueDate)
        {
            var taskId = await impl.Create(description, dueDate);
            var dto = await impl.GetById(taskId);
            Schedule(new TimeDto(dto.DueDate, TimeKind.DueDate));
            return taskId;
        }


        Task IChoreApplicationService.ChangeRecurrenceToDueDate(Guid id, Duration repeatInterval) => impl.ChangeRecurrenceToDueDate(id, repeatInterval);

        Task IChoreApplicationService.ChangeStartDate(Guid taskId, Instant newStartDate) => impl.ChangeStartDate(taskId, newStartDate);

        Task IChoreApplicationService.ChangeRecurrenceToFinishDate(Guid id, Duration repeatInterval) => impl.ChangeRecurrenceToFinishDate(id, repeatInterval);

        Task IChoreApplicationService.ChangeDescription(Guid taskId, string newDescription) => impl.ChangeDescription(taskId, newDescription);

        Task IChoreApplicationService.ChangeLabels(Guid taskId, string newLabels) => impl.ChangeLabels(taskId, newLabels);

        Task IChoreApplicationService.ChangeNotes(Guid taskId, string newNotes) => impl.ChangeNotes(taskId, newNotes);


        Task IChoreApplicationService.ChangePriority(Guid taskId, int newPriority) => impl.ChangePriority(taskId, newPriority);

        Task<ChoreDto> IChoreApplicationService.GetById(Guid choreId) => impl.GetById(choreId);

        Task<IEnumerable<ChoreDto>> IChoreApplicationService.GetChoresFrom(Guid choreId) => impl.GetChoresFrom(choreId);

        Task<Guid?> IChoreApplicationService.Finish(Guid taskId) => impl.Finish(taskId);

        Task IChoreApplicationService.Start(Guid taskId) => impl.Start(taskId);

        Task<IEnumerable<TimeDto>> IChoreApplicationService.PendingTimes() => impl.PendingTimes();
    }
}
