using System.IO;
using System.Linq;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;

namespace Icm.TaskManager.CommandLine
{
    internal static class OutputExtensions
    {
        internal static async System.Threading.Tasks.Task ShowDetails(this TextWriter output, ChoreId choreId, TaskDto task)
        {
            await output.WriteLineAsync($"Task {choreId}: {task.Description}");
            await output.WriteLineAsync($"  Start: {task.StartDate}");
            await output.WriteLineAsync($"  Due: {task.DueDate}");
            await output.WriteLineAsync($"  Finish: {task.FinishDate}");
            await output.WriteLineAsync($"  Priority: {task.Priority}");
            await output.WriteLineAsync($"  Labels: {task.Labels}");
            await output.WriteLineAsync($"  Notes: {task.Notes}");
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

        internal static async System.Threading.Tasks.Task ShowDetailsBrief(this TextWriter output, ChoreId choreId, TaskDto task)
        {
            await output.WriteLineAsync($"Task {choreId}: {task.Description}");
            await output.WriteLnIfAsync($"  Start: {task.StartDate}", task.StartDate.HasValue);
            await output.WriteLineAsync($"  Due: {task.DueDate}");
            await output.WriteLnIfAsync($"  Finish: {task.FinishDate}", task.FinishDate.HasValue);
            await output.WriteLineAsync($"  Priority: {task.Priority}");
            await output.WriteLnIfAsync($"  Labels: {task.Labels}", !string.IsNullOrEmpty(task.Labels));
            await output.WriteLnIfAsync($"  Notes: {task.Notes}", !string.IsNullOrEmpty(task.Notes));
            if (!task.Reminders.Any())
            {
                return;
            }

            await output.WriteLineAsync("  Reminders:");
            foreach (var reminder in task.Reminders)
            {
                await output.WriteLineAsync($"  - {reminder}");
            }
        }

        private static async System.Threading.Tasks.Task WriteLnIfAsync(this TextWriter output, string message, bool condition)
        {
            if (condition)
            {
                await output.WriteLineAsync(message);
            }
        }
    }
}