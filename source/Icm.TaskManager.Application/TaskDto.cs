using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Instant CreationDate { get; set; }

        public Instant? StartDate { get; set; }

        public Instant DueDate { get; set; }

        public Instant? FinishDate { get; set; }

        public Recurrence Recurrence { get; set; }

        public int Priority { get; set; }

        public string Notes { get; set; }

        public string Labels { get; set; }

        public ICollection<Instant> Reminders { get; set; }
    }
}