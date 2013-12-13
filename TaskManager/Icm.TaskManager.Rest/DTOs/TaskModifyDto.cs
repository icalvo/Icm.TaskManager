using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icm.TaskManager.Web.DTOs
{
    /// <summary>
    /// Dto for modifying tasks
    /// </summary>
    public class TaskModifyDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public TimeSpan? RepeatInterval { get; set; }
        public bool RepeatFromDueDate { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public string Labels { get; set; }
    }
}