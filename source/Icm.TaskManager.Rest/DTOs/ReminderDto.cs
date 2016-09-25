using System;

namespace Icm.TaskManager.Rest.DTOs
{
    public class ReminderDto
    {
#pragma warning disable SA1401 // Fields must be private
        public int TaskId;
        public string TaskDescription;
        public DateTime AlertDate;
    }
}