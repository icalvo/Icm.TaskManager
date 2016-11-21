using System;
using System.Linq;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.CommandLine
{
    internal static class OutputExtensions
    {
        internal static void ShowDetails(this IObserver<string> output, TaskId taskId, TaskDto task)
        {
            output.OnNext($"Task {taskId}: {task.Description}");
            output.OnNext($"  Start: {task.StartDate}");
            output.OnNext($"  Due: {task.DueDate}");
            output.OnNext($"  Finish: {task.FinishDate}");
            output.OnNext($"  Priority: {task.Priority}");
            output.OnNext($"  Labels: {task.Labels}");
            output.OnNext($"  Notes: {task.Notes}");
            if (task.Reminders.Any())
            {
                output.OnNext("  Reminders:");
                foreach (var reminder in task.Reminders)
                {
                    output.OnNext($"  - {reminder}");
                }
            }
            else
            {
                output.OnNext("Reminders: NONE");
            }
        }
    }
}