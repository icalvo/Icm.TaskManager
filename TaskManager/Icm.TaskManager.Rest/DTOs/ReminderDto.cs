using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icm.TaskManager.Web.DTOs
{
    public class ReminderDto
    {
        public int TaskId;
        public string TaskDescription;
        public DateTime AlertDate;
    }
}