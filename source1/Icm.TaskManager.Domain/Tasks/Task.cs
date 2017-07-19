﻿using System;
using System.Collections.Generic;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class Task
    {
        private Instant? startDate;
        private Instant? finishDate;

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
                    throw new Exception("Cannot set a task start date after its finish date");
                }

                startDate = value;
            }
        }

        public Instant DueDate { get; set; }

        public Instant? FinishDate
        {
            get
            {
                return finishDate;
            }

            set
            {
                if (IsDone)
                {
                    throw new TaskAlreadyDoneException();
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

        public string Labels { get; set; }

        public ICollection<Instant> Reminders { get; internal set; }

        public bool IsDone => FinishDate.HasValue;

        public Task CopyWithNewDueDate(Instant newDueDate, Instant now)
        {
            var newTask = new Task
            {
                Description = Description,
                CreationDate = now,
                DueDate = newDueDate,
                Recurrence = Recurrence,
                Priority = Priority,
                Notes = Notes,
                Labels = Labels
            };

            return newTask;
        }

        public static Task Create(
            string description,
            Instant dueDate,
            Instant now)
        {
            var newTask = new Task
            {
                Description = description,
                CreationDate = now,
                DueDate = dueDate,
                Priority = 3,
                Reminders = new HashSet<Instant>()
            };

            return newTask;
        }

        public TaskMemento Save()
        {
            return new TaskMemento
            {
                CreationDate = CreationDate,
                Description = Description,
                DueDate = DueDate,
                FinishDate = FinishDate,
                Labels = Labels,
                Notes = Notes,
                Recurrence = Recurrence,
                Reminders = new List<Instant>(Reminders),
                Priority = Priority,
                StartDate = StartDate
            };
        }

        public static Task FromMemento(TaskMemento memento)
        {
            return new Task
            {
                CreationDate = memento.CreationDate,
                Description = memento.Description,
                DueDate = memento.DueDate,
                FinishDate = memento.FinishDate,
                Labels = memento.Labels,
                Notes = memento.Notes,
                Recurrence = memento.Recurrence,
                Reminders = new List<Instant>(memento.Reminders),
                Priority = memento.Priority,
                StartDate = memento.StartDate
            };
        }

        public override string ToString()
        {
            return $"{DueDate:yyyy-MM-dd} {Description}";
        }
    }
}