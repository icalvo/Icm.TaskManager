using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icm.TaskManager.Domain.Tasks
{
    public class Reminder
    {
        public int Id { get; protected set; }
        public DateTime AlarmDate { get; internal set; }
        public Task Task { get; internal set; }
    }
}
