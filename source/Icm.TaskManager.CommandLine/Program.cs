using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Icm.ChoreManager.Application;
using Icm.ChoreManager.CommandLine.Commands;
using Icm.ChoreManager.Domain.Chores;
using Icm.ChoreManager.Infrastructure;
using NodaTime;
using NodaTime.Text;

namespace Icm.ChoreManager.CommandLine
{
    internal static class Program
    {
        private static IChoreApplicationServiceSchedulingAdapter _svc;

        private static readonly Command[] Commands =
        {
            Command.Create(
                "CreateChore",
                "create",
                new Parameter<string>("description", x => CommandParseResult.ParameterSuccess(x, x)),
                new Parameter<Instant>("due date", ParseInstantFunc("yyyy-MM-dd")),
                "creates a chore",
                async (description, dueDate) =>
                {
                    var id = await _svc.CreateAsync(description, dueDate);

                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "AddReminder",
                "reminder",
                new Parameter<ChoreId>("id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                new Parameter<Instant>("reminder time", ParseInstantFunc("yyyy-MM-dd")),
                "creates a chore",
                async (id, reminderTime) =>
                {
                    await _svc.AddReminderAsync(id, reminderTime);

                    await Console.Out.WriteLineAsync("Reminder added!");
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "ChangeDescription",
                "desc",
                new Parameter<ChoreId>("id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                new Parameter<string>("description", x => CommandParseResult.ParameterSuccess(x, x)),
                "creates a chore",
                async (id, description) =>
                {
                    await _svc.ChangeDescriptionAsync(id, description);

                    await Console.Out.WriteLineAsync("Description changed!");
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "ChangeDescription",
                "recdue",
                new Parameter<ChoreId>("id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                new Parameter<Duration>("repeat interval", ParseDurationFunc("HH:mm")),
                "creates a chore",
                async (id, repeatInterval) =>
                {
                    await _svc.SetRecurrenceToDueDateAsync(id, repeatInterval);

                    await Console.Out.WriteLineAsync("Recurrence changed!");
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "ChangeLabels",
                "label",
                new Parameter<ChoreId>("id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                new Parameter<string>("labels", x => CommandParseResult.ParameterSuccess(x, x)),
                "sets labels for a chore",
                async (id, labels) =>
                {
                    await _svc.ChangeLabelsAsync(id, labels);

                    await Console.Out.WriteLineAsync("Labels changed!");
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "ChangeNotes",
                "notes",
                new Parameter<ChoreId>("id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                new Parameter<string>("notes", x => CommandParseResult.ParameterSuccess(x, x)),
                "change notes of a chore",
                async (id, notes) =>
                {
                    await _svc.ChangeNotesAsync(id, notes);

                    await Console.Out.WriteLineAsync("Notes changed!");
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "ListPending",
                "pending",
                "change notes of a chore",
                async () =>
                {
                    var pendingChores = await _svc.GetPendingChoresAsync();

                    foreach (var chore in pendingChores)
                    {
                        await Console.Out.ShowDetailsRow(chore);
                    }
                }),
            Command.Create(
                "ChangePriority",
                "prio",
                new Parameter<ChoreId>("id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                new Parameter<int>("priority", x => CommandParseResult.ParameterSuccess(x, int.Parse(x))),
                "change priority of a chore",
                async (id, priority) =>
                {
                    await _svc.ChangePriorityAsync(id, priority);

                    await Console.Out.WriteLineAsync("Priority changed!");
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "ShowChore",
                "show",
                new Parameter<ChoreId>("chore id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                "shows chore details",
                async id =>
                {
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                }),
            Command.Create(
                "FinishChore",
                "finish",
                new Parameter<ChoreId>("chore id", x => CommandParseResult.ParameterSuccess(x, ChoreId.Parse(x))),
                "finishes a chore",
                async id =>
                {
                    ChoreId? newid = await _svc.FinishAsync(id);
                    var dto = await _svc.GetByIdAsync(id);
                    await Console.Out.ShowDetailsBrief(dto);
                    if (newid.HasValue)
                    {
                        await Console.Out.WriteLineAsync("A new recurrence has been created");
                        dto = await _svc.GetByIdAsync(newid.Value);
                        await Console.Out.ShowDetailsBrief(dto);
                    }
                })
        };

        private static Func<string, CommandParseResult<Instant>> ParseInstantFunc(
            string format,
            string errorMessage = "Invalid date")
        {
            return token =>
            {
                var parseResult = InstantPattern
                    .CreateWithInvariantCulture(format).Parse(token);


                if (parseResult.Success)
                {
                    return CommandParseResult.ParameterSuccess(token, parseResult.Value);
                }

                return CommandParseResult.ParameterError<Instant>(token, errorMessage);
            };
        }

        private static Func<string, CommandParseResult<Duration>> ParseDurationFunc(
            string format,
            string errorMessage = "Invalid duration")
        {
            return token =>
            {
                var parseResult = DurationPattern
                    .CreateWithInvariantCulture(format).Parse(token);


                if (parseResult.Success)
                {
                    return CommandParseResult.ParameterSuccess(token, parseResult.Value);
                }

                return CommandParseResult.ParameterError<Duration>(token, errorMessage);
            };
        }

        internal static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            //// IChoreRepository RepoBuilder() => new DapperChoreRepository(ConnectionStrings.ByName("Trolo"));
            IChoreRepository RepoBuilder() => InMemoryChoreRepository.WithStaticStorage();
            //// IChoreRepository RepoBuilder() => new AzureChoreRepository();

            IChoreApplicationService basicSvc = new ChoreApplicationService(RepoBuilder, SystemClock.Instance);

            var timerExpirations = new Subject<TimeDto>();
            var timerStarts = new Subject<TimeDto>();
            _svc = new SchedulingAdapter(
                basicSvc,
                TaskPoolScheduler.Default,
                timerExpirations);

            timerStarts
                .Select(dto => dto.ToString())
                .Subscribe(x => Console.WriteLine($"Timer started, due {x}"));

            timerExpirations
                .Select(dto => dto.ToString())
                .Subscribe(x => Console.WriteLine($"Timer expired"));

            await _svc.ScheduleExistingAsync();

            var runner = new CommandRunner(Commands);

            CommandRunner.QuitCommand.Verbs.Add("out");
            await runner.Run();
        }
    }
}
