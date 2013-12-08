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
        public DateTime? DueDate { get; internal set; }
        public DateTime? FinishDate { get; internal set; }
        public TimeSpan? RepeatInterval { get; internal set; }
        public bool RepeatFromDueDate { get; internal set; }
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

        public override string ToString()
        {
            return string.Format("{0} {2:yyyy-MM-dd} {1}", this.Id, this.Description, this.StartDate);
        }
    }
}
