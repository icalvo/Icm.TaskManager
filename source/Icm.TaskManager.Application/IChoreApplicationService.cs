using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public interface IChoreApplicationService
    {
        Task<Guid> Create(string description, Instant dueDate);

        Task AddReminder(Guid choreId, Instant reminder);

        Task ChangeRecurrenceToDueDate(Guid choreId, Duration repeatInterval);

        Task ChangeRecurrenceToFinishDate(Guid choreId, Duration repeatInterval);

        Task ChangeDescription(Guid choreId, string newDescription);

        Task ChangeLabels(Guid choreId, string newLabels);

        Task ChangeNotes(Guid choreId, string newNotes);

        Task ChangePriority(Guid choreId, int newPriority);

        Task ChangeStartDate(Guid choreId, Instant newStartDate);

        Task ChangeDueDate(Guid choreId, Instant newDueDate);

        Task<ChoreDto> GetById(Guid choreId);

        Task<IEnumerable<ChoreDto>> GetChoresFrom(Guid choreId);

        Task<Guid?> Finish(Guid choreId);

        Task Start(Guid choreId);

        Task<IEnumerable<TimeDto>> PendingTimes();
    }
}