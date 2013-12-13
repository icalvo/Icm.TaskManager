using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain.Tasks
{
    public class Task
    {
        public int Id { get; protected set; }
        public string Description { get; set; }

        public DateTime? CreationDate { get; internal set; }
        public DateTime? StartDate { get; internal set; }
        public DateTime DueDate { get; internal set; }
        public DateTime? FinishDate { get; internal set; }

        public Recurrence Recurrence
        {
            get {
                Type recurrenceType = System.Reflection.Assembly.GetExecutingAssembly().DefinedTypes.SingleOrDefault(type => type.Name == this.RecurrenceType);


                if (recurrenceType != null && this.RepeatInterval.HasValue)
                {
                    var recurrence = (Recurrence)Activator.CreateInstance(recurrenceType);
                    recurrence.RepeatInterval = this.RepeatInterval.Value;
                    return recurrence;
                }
                else {
                    return null;
                }
            }
            set {
                if (value == null) {
                    this.RecurrenceType = null;
                }
                else {
                    this.RecurrenceType = value.GetType().Name;
                    this.RepeatInterval = value.RepeatInterval;
                }
            }
        }

        public string RecurrenceType { get; set; }
        public TimeSpan? RepeatInterval { get; set; }
        public int Priority { get; internal set; }
        public string Notes { get; set; }
        public string Labels { get; set; }

        public ICollection<Reminder> Reminders { get; internal set; }

        public bool IsDone
        {
            get
            {
                return FinishDate.HasValue;
            }
        }

        internal void SetFinishDate(DateTime finishDate) {
            this.FinishDate = finishDate;
        }

        public Task CopyWithNewDueDate(DateTime newDueDate, ICurrentDateProvider currentDateProvider)
        {
            var newTask = new Task();

            newTask.Description = this.Description;
            newTask.CreationDate = currentDateProvider.Now;
            newTask.DueDate = newDueDate;
            newTask.Recurrence = this.Recurrence;
            newTask.Priority = this.Priority;
            newTask.Notes = this.Notes;
            newTask.Labels = this.Labels;

            return newTask;
        }

        public override string ToString()
        {
            return string.Format("{0} {2:yyyy-MM-dd} {1}", this.Id, this.Description, this.StartDate);
        }
    }
}
