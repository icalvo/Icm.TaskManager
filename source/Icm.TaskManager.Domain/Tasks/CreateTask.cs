using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class CreateTask : Command
    {
        public string Description { get; }

        public Instant DueDate { get; set; }
    }
}