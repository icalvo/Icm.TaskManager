namespace Icm.TaskManager.Domain.Tasks
{
    public class TaskNotesChangedEvent
    {
        public int TaskId { get; }

        public string Notes { get; }

        public TaskNotesChangedEvent(int taskId, string notes)
        {
            this.TaskId = taskId;
            this.Notes = notes;
        }
    }
}