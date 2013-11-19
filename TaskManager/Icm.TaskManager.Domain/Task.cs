using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Domain
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public TimeSpan? RepeatInterval { get; set; }
        public bool RepeatFromDueDate { get; set; }
        public int Priority { get; set; }
        public DateTime? AlarmDate { get; set; }
        public string Notes { get; set; }

        public string Labels { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {2:yyyy-MM-dd} {1}", this.Id, this.Description, this.StartDate);
        }
    }
}
