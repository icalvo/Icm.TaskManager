﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface ITaskService
    {
        Task CreateTask(string description, Instant? startDate, Instant dueDate, string recurrenceType, Duration? repeatInterval, int priority, string notes, string labels);

        Task Finish(Task task);
    }
}