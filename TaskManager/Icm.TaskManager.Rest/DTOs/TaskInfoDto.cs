﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icm.TaskManager.Rest.DTOs
{
    /// <summary>
    /// DTO for getting task information
    /// </summary>
    public class TaskInfoDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public TimeSpan? RepeatInterval { get; set; }
        public string RecurrenceType { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public string Labels { get; set; }
    }
}