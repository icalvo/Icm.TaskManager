using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public struct ReminderAddedEvent
    {
        public ReminderAddedEvent(int taskId, Instant alarmInstant)
        {
            TaskId = taskId;
            AlarmInstant = alarmInstant;
        }

        public int TaskId { get; }

        public Instant AlarmInstant { get; }

        public override string ToString()
        {
            return $"{GetType().Name} {TaskId} {AlarmInstant}";
        }
    }
}