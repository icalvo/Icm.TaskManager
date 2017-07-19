using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public interface IChoreApplicationService
    {
        Task AddReminder(int taskId, Instant reminder);

        Task ChangeRecurrenceToDueDate(int id, Duration repeatInterval);

        Task ChangeRecurrenceToFinishDate(int id, Duration repeatInterval);

        Task ChangeDescription(int taskId, string newDescription);

        Task ChangeLabels(int taskId, string newLabels);

        Task ChangeNotes(int taskId, string newNotes);

        Task ChangePriority(int taskId, int newPriority);

        Task ChangeDueDate(int taskId, Instant newDueDate);

        Task<int> Create(string description, Instant dueDate);

        Task<TaskDto> GetById(int taskId);

        Task<IEnumerable<TaskDto>> GetTasks(int taskIdFrom);

        Task<int?> Finish(int taskId);

        Task Start(int taskId);

        Task<IEnumerable<ChoreDto>> PendingTimes();
    }
}