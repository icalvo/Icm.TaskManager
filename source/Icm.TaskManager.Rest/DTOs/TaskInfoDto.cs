using NodaTime;

namespace Icm.TaskManager.Rest.DTOs
{
    /// <summary>
    /// DTO for getting task information
    /// </summary>
    public class TaskInfoDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Instant? CreationDate { get; set; }

        public Instant? StartDate { get; set; }

        public Instant DueDate { get; set; }

        public Instant? FinishDate { get; set; }

        public Duration? RepeatInterval { get; set; }

        public string RecurrenceType { get; set; }

        public int Priority { get; set; }

        public string Notes { get; set; }

        public string Labels { get; set; }
    }
}