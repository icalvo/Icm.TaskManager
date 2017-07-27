using System.Collections.Generic;
using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public class ChoreMemento
    {
        public ChoreId Id { get; set; }

        public string Description { get; set; }

        public Instant CreationDate { get; set; }

        public Instant? StartDate { get; set; }

        public Instant DueDate { get; set; }

        public Instant? FinishDate { get; set; }

        public RecurrenceMemento Recurrence { get; set; }

        public int Priority { get; set; }

        public string Notes { get; set; }

        public string[] Labels { get; set; }

        public List<Instant> Reminders { get; set; }
    }

    public class RecurrenceMemento
    {
        public RecurrenceKind Kind { get; set; }
        public Duration Interval { get; set; }
    }
}