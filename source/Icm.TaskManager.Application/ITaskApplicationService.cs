using System.Collections.Generic;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public interface ITaskApplicationService
    {
        void AddTaskReminder(int taskId, Instant reminder);

        void AddTaskReminderRelativeToNow(int taskId, Duration offset);

        void ChangeRecurrenceToDueDate(int id, Duration repeatInterval);

        void ChangeRecurrenceToFinishDate(int id, Duration repeatInterval);

        void ChangeTaskDescription(int taskId, string newDescription);

        void ChangeTaskLabels(int taskId, string newLabels);

        void ChangeTaskNotes(int taskId, string newNotes);

        void ChangeTaskPriority(int taskId, int newPriority);

        int CreateTask(string description, Instant dueDate);

        TaskDto GetTaskById(int taskId);

        IEnumerable<TaskDto> GetTasks();

        int? FinishTask(int taskId);

        void StartTask(int taskId);
    }
}