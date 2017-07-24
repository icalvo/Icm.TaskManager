using System;
using System.Collections.Generic;
using Icm.ChoreManager.Domain.Chores;
using NodaTime;
using Microsoft.WindowsAzure.Storage.Table;
using NodaTime.Text;

namespace Icm.ChoreManager.Infrastructure
{
    public class ChoreEntity : TableEntity
    {
        public ChoreEntity(Chore chore)
        {
            var pattern = InstantPattern.General;
            Id = Guid.NewGuid();
            Description = chore.Description;
            CreationDate = pattern.Format(chore.CreationDate);
            StartDate = chore.StartDate.HasValue? pattern.Format(chore.StartDate.Value) : string.Empty;
            DueDate = pattern.Format(chore.DueDate);
            FinishDate = chore.FinishDate.HasValue ? pattern.Format(chore.FinishDate.Value) : string.Empty;
            Priority = chore.Priority;
            Recurrence = chore.Recurrence;
            Notes = chore.Notes;
            Labels = chore.Labels;
            Reminders = chore.Reminders;
        }

        public ChoreEntity(ChoreId id, Chore chore) : this(chore)
        {
            Id = id;
        }

        public ChoreEntity() { }

        private Guid id;

        public Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                PartitionKey = id.ToString();
                RowKey = id.ToString();
            }
        }

        public string Description { get; set; }

        public string CreationDate { get; set; }

        public string StartDate { get; set; }

        public string DueDate { get; set; }

        public string FinishDate { get; set; }

        public Recurrence Recurrence { get; set; }

        public int Priority { get; set; }

        public string Notes { get; set; }

        public string Labels { get; set; }

        public ICollection<Instant> Reminders { get; set; }


        public ChoreMemento ToMemento(ChoreId id)
        {
            return new ChoreMemento
            {
                Id = id.Value,
                CreationDate = Parse(CreationDate).Value,
                Description = Description,
                DueDate = Parse(DueDate).Value,
                FinishDate = Parse(FinishDate),
                Labels = Labels,
                Notes = Notes,
                Recurrence = Recurrence,
                Reminders = Reminders == null ? new List<Instant>() : new List<Instant>(Reminders),
                Priority = Priority,
                StartDate = Parse(StartDate)
            };
        }

        private static Instant? Parse(string instant)
        {
            var result = InstantPattern.General.Parse(instant);

            if (result.Success)
            {
                return result.Value;
            }

            return null;
        }
    }
}