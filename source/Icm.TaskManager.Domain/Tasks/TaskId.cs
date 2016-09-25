namespace Icm.TaskManager.Domain.Tasks
{
    public struct TaskId
    {
        public int Value { get; }

        public TaskId(int id)
        {
            Value = id;
        }

        public static implicit operator int(TaskId id)
        {
            return id.Value;
        }
    }
}