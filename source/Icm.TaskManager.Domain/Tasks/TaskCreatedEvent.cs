using System;
using NodaTime;

namespace Icm.TaskManager.Application
{
    public class TaskCreatedEvent
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public Instant DueDate { get; set; }

        public Instant CreationInstant { get; set; }

        public TaskCreatedEvent(Guid id, string description, Instant dueDate, Instant creationInstant)
        {
            Id = id;
            Description = description;
            DueDate = dueDate;
            CreationInstant = creationInstant;
        }
    }
}