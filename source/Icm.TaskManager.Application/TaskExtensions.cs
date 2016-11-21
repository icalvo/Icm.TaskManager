﻿using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Application
{
    internal static class TaskExtensions
    {
        internal static TaskDto ToDto(this Identified<TaskId, Task> idtask)
        {
            var task = idtask.Value;
            return new TaskDto
            {
                Id = idtask.Id,
                CreationDate = task.CreationDate,
                Description = task.Description,
                DueDate = task.DueDate,
                FinishDate = task.FinishDate,
                Labels = task.Labels,
                Notes = task.Notes,
                Recurrence = task.Recurrence,
                Reminders = new List<Instant>(task.Reminders),
                Priority = task.Priority,
                StartDate = task.StartDate
            };
        }
    }
}