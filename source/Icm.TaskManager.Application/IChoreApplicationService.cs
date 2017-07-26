using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;

namespace Icm.ChoreManager.Application
{
    public interface IChoreApplicationService
    {
        Task<ChoreId> CreateAsync(string description, Instant dueDate);

        Task AddReminderAsync(ChoreId choreId, Instant reminder);

        Task SetRecurrenceToDueDateAsync(ChoreId choreId, Duration repeatInterval);

        Task SetRecurrenceToFinishDateAsync(ChoreId choreId, Duration repeatInterval);

        Task RemoveRecurrenceAsync(ChoreId choreId);

        Task ChangeDescriptionAsync(ChoreId choreId, string newDescription);

        Task ChangeLabelsAsync(ChoreId choreId, string newLabels);

        Task ChangeNotesAsync(ChoreId choreId, string newNotes);

        Task ChangePriorityAsync(ChoreId choreId, int newPriority);

        Task ChangeStartDateAsync(ChoreId choreId, Instant newStartDate);

        Task ChangeDueDateAsync(ChoreId choreId, Instant newDueDate);

        Task<ChoreDto> GetByIdAsync(ChoreId choreId);

        Task<IEnumerable<ChoreDto>> GetPendingChoresAsync();

        Task<ChoreId?> FinishAsync(ChoreId choreId);

        Task StartAsync(ChoreId choreId);

        Task<IEnumerable<TimeDto>> PendingTimesAsync();
    }
}