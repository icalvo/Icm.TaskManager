using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain;
using NodaTime;

namespace Icm.TaskManager.CommandLine
{
    public class TaskApplicationServiceSchedulingAdapter : ITaskApplicationService
    {
        private readonly ITaskApplicationService impl;
        private readonly IScheduler scheduler;
        private readonly Subject<TimeDto> timeChanges;
        private readonly IDictionary<Instant, IDisposable> pending;

        public TaskApplicationServiceSchedulingAdapter(
            ITaskApplicationService impl,
            IScheduler scheduler)
        {
            this.impl = impl;
            this.scheduler = scheduler;
            timeChanges = new Subject<TimeDto>();
            pending = new Dictionary<Instant, IDisposable>();

            impl.PendingTimes().Execute(Schedule);
        }

        public IObservable<TimeDto> TimeChanges => timeChanges;

        private void Schedule(TimeDto pendingTime)
        {
            pending.Add(pendingTime.Time, scheduler.Schedule(pendingTime.Time.ToDateTimeOffset(), () => timeChanges.OnNext(pendingTime)));
        }

        private void Unschedule(Instant instant)
        {
            pending[instant].Dispose();
        }

        void ITaskApplicationService.AddTaskReminder(int taskId, Instant reminder)
        {
            impl.AddTaskReminder(taskId, reminder);
            Schedule(new TimeDto(reminder, TimeKind.Reminder));
        }

        void ITaskApplicationService.ChangeRecurrenceToDueDate(int id, Duration repeatInterval)
        {
            impl.ChangeRecurrenceToDueDate(id, repeatInterval);
        }

        void ITaskApplicationService.ChangeRecurrenceToFinishDate(int id, Duration repeatInterval)
        {
            impl.ChangeRecurrenceToFinishDate(id, repeatInterval);
        }

        void ITaskApplicationService.ChangeTaskDescription(int taskId, string newDescription)
        {
            impl.ChangeTaskDescription(taskId, newDescription);
        }

        void ITaskApplicationService.ChangeTaskLabels(int taskId, string newLabels)
        {
            impl.ChangeTaskLabels(taskId, newLabels);
        }

        void ITaskApplicationService.ChangeTaskNotes(int taskId, string newNotes)
        {
            impl.ChangeTaskNotes(taskId, newNotes);
        }

        void ITaskApplicationService.ChangeTaskPriority(int taskId, int newPriority)
        {
            impl.ChangeTaskPriority(taskId, newPriority);
        }

        public void ChangeTaskDueDate(int taskId, Instant newDueDate)
        {
            var dto = impl.GetTaskById(taskId);
            impl.ChangeTaskDueDate(taskId, newDueDate);
            Unschedule(dto.DueDate);
            Schedule(new TimeDto(newDueDate, TimeKind.DueDate));
        }

        int ITaskApplicationService.CreateTask(string description, Instant dueDate)
        {
            var taskId = impl.CreateTask(description, dueDate);
            var dto = impl.GetTaskById(taskId);
            Schedule(new TimeDto(dto.DueDate, TimeKind.DueDate));
            dto.Reminders.Execute(reminder => Schedule(new TimeDto(reminder, TimeKind.Reminder)));
            return taskId;
        }

        TaskDto ITaskApplicationService.GetTaskById(int taskId)
        {
            return impl.GetTaskById(taskId);
        }

        IEnumerable<TaskDto> ITaskApplicationService.GetTasks()
        {
            return impl.GetTasks();
        }

        int? ITaskApplicationService.FinishTask(int taskId)
        {
            return impl.FinishTask(taskId);
        }

        void ITaskApplicationService.StartTask(int taskId)
        {
            impl.StartTask(taskId);
        }

        IEnumerable<TimeDto> ITaskApplicationService.PendingTimes()
        {
            return impl.PendingTimes();
        }
    }
}
