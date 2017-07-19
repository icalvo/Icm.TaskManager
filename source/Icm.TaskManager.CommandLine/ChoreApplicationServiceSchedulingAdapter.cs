using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
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
        private readonly IObserver<ChoreDto> timerStarts;
        private readonly IObserver<ChoreDto> timerExpirations;
        private readonly IDictionary<Instant, IDisposable> pending;

        public ChoreApplicationServiceSchedulingAdapter(
            IChoreApplicationService impl,
            IScheduler scheduler,
            IObserver<ChoreDto> timerStarts,
            IObserver<ChoreDto> timerExpirations)
        {
            this.impl = impl;
            this.scheduler = scheduler;
            this.timerStarts = timerStarts;
            this.timerExpirations = timerExpirations;
            pending = new Dictionary<Instant, IDisposable>();
        }

        public async Task ScheduleExisting()
        {
            var pendingTimes = await impl.PendingTimes();
            pendingTimes.Execute(Schedule);
        }

        private void Schedule(ChoreDto pendingChore)
        {
            var scheduleHandler = scheduler.Schedule(
                pendingChore.Time.ToDateTimeOffset(),
                () => timerExpirations.OnNext(pendingChore));
            pending.Add(pendingChore.Time, scheduleHandler);
            timerStarts.OnNext(pendingChore);
        }

        private void Unschedule(Instant instant)
        {
            var scheduleHandler = pending[instant];
            scheduleHandler.Dispose();
        }

        async Task IChoreApplicationService.AddReminder(int taskId, Instant reminder)
        {
            await impl.AddReminder(taskId, reminder);
            Schedule(new ChoreDto(reminder, TimeKind.Reminder));
        }

        Task IChoreApplicationService.ChangeRecurrenceToDueDate(int id, Duration repeatInterval)
        {
            return impl.ChangeRecurrenceToDueDate(id, repeatInterval);
        }

        Task IChoreApplicationService.ChangeRecurrenceToFinishDate(int id, Duration repeatInterval)
        {
            return impl.ChangeRecurrenceToFinishDate(id, repeatInterval);
        }

        Task IChoreApplicationService.ChangeDescription(int taskId, string newDescription)
        {
            return impl.ChangeDescription(taskId, newDescription);
        }

        Task IChoreApplicationService.ChangeLabels(int taskId, string newLabels)
        {
            return impl.ChangeLabels(taskId, newLabels);
        }

        Task IChoreApplicationService.ChangeNotes(int taskId, string newNotes)
        {
            return impl.ChangeNotes(taskId, newNotes);
        }

        Task IChoreApplicationService.ChangePriority(int taskId, int newPriority)
        {
            return impl.ChangePriority(taskId, newPriority);
        }

        public async Task ChangeDueDate(int taskId, Instant newDueDate)
        {
            var dto = await impl.GetById(taskId);
            await impl.ChangeDueDate(taskId, newDueDate);
            Unschedule(dto.DueDate);
            Schedule(new ChoreDto(newDueDate, TimeKind.DueDate));
        }

        async Task<int> IChoreApplicationService.Create(string description, Instant dueDate)
        {
            var taskId = await impl.Create(description, dueDate);
            var dto = await impl.GetById(taskId);
            Schedule(new ChoreDto(dto.DueDate, TimeKind.DueDate));
            return taskId;
        }

        Task<TaskDto> IChoreApplicationService.GetById(int taskId)
        {
            return impl.GetById(taskId);
        }

        Task<IEnumerable<TaskDto>> IChoreApplicationService.GetTasks(int taskIdFrom)
        {
            return impl.GetTasks(taskIdFrom);
        }

        Task<int?> IChoreApplicationService.Finish(int taskId)
        {
            return impl.Finish(taskId);
        }

        Task IChoreApplicationService.Start(int taskId)
        {
            return impl.Start(taskId);
        }

        Task<IEnumerable<ChoreDto>> IChoreApplicationService.PendingTimes()
        {
            return impl.PendingTimes();
        }
    }
}
