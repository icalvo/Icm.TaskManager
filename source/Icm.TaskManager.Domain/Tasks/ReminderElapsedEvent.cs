namespace Icm.TaskManager.Domain.Tasks
{
    public struct ReminderElapsedEvent
    {
        public ReminderElapsedEvent(int taskId)
        {
            TaskId = taskId;
        }

        public int TaskId { get; }
    }
}