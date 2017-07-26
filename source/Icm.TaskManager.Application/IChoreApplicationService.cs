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

        Task ChangeRecurrenceToDueDateAsync(ChoreId choreId, Duration repeatInterval);

        Task ChangeRecurrenceToFinishDateAsync(ChoreId choreId, Duration repeatInterval);

        Task ChangeDescriptionAsync(ChoreId choreId, string newDescription);

        Task ChangeLabelsAsync(ChoreId choreId, string newLabels);

        Task ChangeNotesAsync(ChoreId choreId, string newNotes);

        Task ChangePriorityAsync(ChoreId choreId, int newPriority);

        Task ChangeStartDateAsync(ChoreId choreId, Instant newStartDate);

        Task ChangeDueDateAsync(ChoreId choreId, Instant newDueDate);

        Task<ChoreDto> GetByIdAsync(ChoreId choreId);

        Task<IEnumerable<ChoreDto>> GetActiveChoresAsync();

        Task<ChoreId?> FinishAsync(ChoreId choreId);

        Task StartAsync(ChoreId choreId);

        Task<IEnumerable<TimeDto>> PendingTimesAsync();
    }
}