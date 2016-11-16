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

        int CreateDueDateRecurringTask(string description, Instant dueDate, Duration repeatInterval, int priority, string notes, string labels);

        int CreateFinishDateRecurringTask(string description, Instant dueDate, Duration repeatInterval, int priority, string notes, string labels);

        int CreateSimpleTask(string description, Instant dueDate);

        int CreateTaskParsing(string description, Instant dueDate, string recurrenceType, Duration? repeatInterval, int priority, string notes, string labels);

        int? FinishTask(int taskId);

        void StartTask(int taskId);
    }
}