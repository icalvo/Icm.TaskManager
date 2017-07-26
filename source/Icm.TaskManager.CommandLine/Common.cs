using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Icm.ChoreManager.Application;
using Icm.ChoreManager.Domain.Chores;

namespace Icm.ChoreManager.CommandLine
{
    internal static class OutputExtensions
    {
        internal static async Task ShowDetails(this TextWriter output, ChoreId choreId, ChoreDto chore)
        {
            await output.WriteLineAsync($"Task {choreId}: {chore.Description}");
            await output.WriteLineAsync($"  Start: {chore.StartDate}");
            await output.WriteLineAsync($"  Due: {chore.DueDate}");
            await output.WriteLineAsync($"  Finish: {chore.FinishDate}");
            await output.WriteLineAsync($"  Priority: {chore.Priority}");
            await output.WriteLineAsync($"  Labels: {chore.Labels}");
            await output.WriteLineAsync($"  Notes: {chore.Notes}");
            if (chore.Reminders.Any())
            {
                output.WriteLine("  Reminders:");
                foreach (var reminder in chore.Reminders)
                {
                    output.WriteLine($"  - {reminder}");
                }
            }
            else
            {
                output.WriteLine("Reminders: NONE");
            }
        }

        internal static async Task ShowDetailsBrief(this TextWriter output, ChoreDto chore)
        {
            await output.WriteLineAsync($"Task {chore.Id}: {chore.Description}");
            await output.WriteLnIfAsync($"  Start: {chore.StartDate}", chore.StartDate.HasValue);
            await output.WriteLineAsync($"  Due: {chore.DueDate}");
            await output.WriteLnIfAsync($"  Finish: {chore.FinishDate}", chore.FinishDate.HasValue);
            await output.WriteLineAsync($"  Priority: {chore.Priority}");
            await output.WriteLnIfAsync($"  Labels: {chore.Labels}", !string.IsNullOrEmpty(chore.Labels));
            await output.WriteLnIfAsync($"  Notes: {chore.Notes}", !string.IsNullOrEmpty(chore.Notes));
            if (!chore.Reminders.Any())
            {
                return;
            }

            await output.WriteLineAsync("  Reminders:");
            foreach (var reminder in chore.Reminders)
            {
                await output.WriteLineAsync($"  - {reminder}");
            }
        }

        internal static async Task ShowDetailsRow(this TextWriter output, ChoreDto chore)
        {
            await output.WriteLineAsync($"Task {chore.Id}: {chore.Description}");
        }

        private static async Task WriteLnIfAsync(this TextWriter output, string message, bool condition)
        {
            if (condition)
            {
                await output.WriteLineAsync(message);
            }
        }
    }
}