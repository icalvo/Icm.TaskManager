using System.IO;
using System.Linq;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.CommandLine
{
    internal static class OutputExtensions
    {
        internal static void ShowDetails(this TextWriter output, TaskId taskId, TaskDto task)
        {
            output.WriteLine($"Task {taskId}: {task.Description}");
            output.WriteLine($"  Start: {task.StartDate}");
            output.WriteLine($"  Due: {task.DueDate}");
            output.WriteLine($"  Finish: {task.FinishDate}");
            output.WriteLine($"  Priority: {task.Priority}");
            output.WriteLine($"  Labels: {task.Labels}");
            output.WriteLine($"  Notes: {task.Notes}");
            if (task.Reminders.Any())
            {
                output.WriteLine("  Reminders:");
                foreach (var reminder in task.Reminders)
                {
                    output.WriteLine($"  - {reminder}");
                }
            }
            else
            {
                output.WriteLine("Reminders: NONE");
            }
        }

        internal static void ShowDetailsBrief(this TextWriter output, TaskId taskId, TaskDto task)
        {
            output.WriteLine($"Task {taskId}: {task.Description}");
            output.WriteLnIf($"  Start: {task.StartDate}", task.StartDate.HasValue);
            output.WriteLine($"  Due: {task.DueDate}");
            output.WriteLnIf($"  Finish: {task.FinishDate}", task.FinishDate.HasValue);
            output.WriteLine($"  Priority: {task.Priority}");
            output.WriteLnIf($"  Labels: {task.Labels}", !string.IsNullOrEmpty(task.Labels));
            output.WriteLnIf($"  Notes: {task.Notes}", !string.IsNullOrEmpty(task.Notes));
            if (!task.Reminders.Any())
            {
                return;
            }

            output.WriteLine("  Reminders:");
            foreach (var reminder in task.Reminders)
            {
                output.WriteLine($"  - {reminder}");
            }
        }

        private static void WriteLnIf(this TextWriter output, string message, bool condition)
        {
            if (condition)
            {
                output.WriteLine(message);
            }
        }
    }
}