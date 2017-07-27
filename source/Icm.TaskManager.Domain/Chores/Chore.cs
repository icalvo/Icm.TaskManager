using System;
using System.Collections.Generic;
using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public class Chore
    {
        private Instant? startDate;
        private Instant? finishDate;

        private Chore(
            string description,
            Instant creationDate,
            Instant dueDate,
            Instant? finishDate,
            Recurrence recurrence,
            int priority,
            string notes,
            string[] labels,
            ICollection<Instant> reminders,
            Instant? startDate)
        {
            Description = description;
            CreationDate = creationDate;
            DueDate = dueDate;
            FinishDate = finishDate;
            Recurrence = recurrence;
            Priority = priority;
            Notes = notes;
            Labels = labels;
            Reminders = reminders;
            StartDate = startDate;
        }

        public string Description { get; set; }

        public Instant CreationDate { get; private set; }

        public Instant? StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                if (value.HasValue && value > finishDate)
                {
                    throw new Exception("Cannot set a chore start date after its finish date");
                }

                startDate = value;
            }
        }

        public Instant DueDate { get; set; }

        public Instant? FinishDate
        {
            get { return finishDate; }

            set
            {
                if (IsDone)
                {
                    throw new ChoreAlreadyFinishedException();
                }

                if (StartDate.HasValue && StartDate > value)
                {
                    throw new Exception();
                }

                finishDate = value;
            }
        }

        public Recurrence Recurrence { get; set; }

        public int Priority { get; set; }

        public string Notes { get; set; }

        public string[] Labels { get; set; }

        public ICollection<Instant> Reminders { get; }

        public bool IsDone => FinishDate.HasValue;

        public Chore CopyWithNewDueDate(Instant newDueDate, Instant now) =>
            new Chore(
                description: Description,
                creationDate: now,
                dueDate: newDueDate,
                finishDate: null,
                recurrence: Recurrence,
                priority: Priority,
                notes: Notes,
                labels: Labels,
                reminders: Reminders,
                startDate: null);

        public static Chore Create(
            string description,
            Instant dueDate,
            Instant creationDate) =>
        new Chore(
            description: description,
            creationDate: creationDate,
            dueDate: dueDate,
            finishDate: null,
            recurrence: null,
            priority: 3,
            notes: null,
            labels: new string[0], 
            reminders: new HashSet<Instant>(),
            startDate: null);

        public ChoreMemento ToMemento(ChoreId id)
        {
            return new ChoreMemento
            {
                Id = id.Value,
                CreationDate = CreationDate,
                Description = Description,
                DueDate = DueDate,
                FinishDate = FinishDate,
                Labels = Labels,
                Notes = Notes,
                Recurrence = Recurrence == null ? null : new RecurrenceMemento { Kind = Recurrence.Kind, Interval = Recurrence.RepeatInterval },
                Reminders = new List<Instant>(Reminders),
                Priority = Priority,
                StartDate = StartDate
            };
        }

        public static Chore FromMemento(ChoreMemento memento)
        {
            return new Chore(
                description: memento.Description,
                creationDate: memento.CreationDate,
                dueDate: memento.DueDate,
                finishDate: memento.FinishDate,
                labels: memento.Labels ?? new string[0],
                notes: memento.Notes,
                recurrence: memento.Recurrence == null ? null : Recurrence.FromMemento(memento.Recurrence),
                reminders: memento.Reminders == null ? new List<Instant>() : new List<Instant>(memento.Reminders),
                priority: memento.Priority,
                startDate: memento.StartDate);
        }

        public override string ToString()
        {
            return $"{DueDate:yyyy-MM-dd} {Description}";
        }
    }
}
