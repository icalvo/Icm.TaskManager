using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icm.TaskManager.Rest.DTOs
{
    public class ReminderDto
    {
        public int TaskId;
        public string TaskDescription;
        public DateTime AlertDate;
    }
}