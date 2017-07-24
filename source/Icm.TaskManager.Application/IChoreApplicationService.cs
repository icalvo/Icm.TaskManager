using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.ChoreManager.Application
{
    public interface IChoreApplicationService
    {
        Task<Guid> CreateAsync(string description, Instant dueDate);

        Task AddReminderAsync(Guid choreId, Instant reminder);

        Task ChangeRecurrenceToDueDateAsync(Guid choreId, Duration repeatInterval);

        Task ChangeRecurrenceToFinishDateAsync(Guid choreId, Duration repeatInterval);

        Task ChangeDescriptionAsync(Guid choreId, string newDescription);

        Task ChangeLabelsAsync(Guid choreId, string newLabels);

        Task ChangeNotesAsync(Guid choreId, string newNotes);

        Task ChangePriorityAsync(Guid choreId, int newPriority);

        Task ChangeStartDateAsync(Guid choreId, Instant newStartDate);

        Task ChangeDueDateAsync(Guid choreId, Instant newDueDate);

        Task<ChoreDto> GetByIdAsync(Guid choreId);

        Task<IEnumerable<ChoreDto>> GetChoresFromAsync(Guid choreId);

        Task<Guid?> FinishAsync(Guid choreId);

        Task StartAsync(Guid choreId);

        Task<IEnumerable<TimeDto>> PendingTimesAsync();
    }
}