using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Icm.TaskManager.Application;
using Icm.TaskManager.CommandLine.Commands;
using Icm.TaskManager.Domain.Chores;
using Icm.TaskManager.Infrastructure;
using NodaTime;
using NodaTime.Text;

namespace Icm.TaskManager.CommandLine
{
    internal static class Program
    {
        private static IChoreApplicationService _svc;

        private static readonly Command CreateChoreCommand = new Command(
            "CreateChore",
            "create",
            new[] {
                new Parameter("description"),
                new Parameter("due date", arg => Parameter.DateParses(arg, "yyyy-MM-dd")) },
            "creates a chore",
            async tokens =>
            {
                await Console.Out.WriteLineAsync("Creating task...");

                var description = tokens[1];
                var dueDate = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd", DateTimeZoneProviders.Tzdb).Parse(tokens[2]).Value.ToInstant();
                var id = await _svc.Create(description, dueDate);
                await Console.Out.WriteLineAsync($"Task {id} created");
                var dto = await _svc.GetById(id);
                await Console.Out.ShowDetailsBrief(id, dto);
            });

        private static readonly Command ShowChoreCommand = new Command(
            "ShowChore",
            "show",
            new []
            {
                new Parameter("chore id", Parameter.GuidParses), 
            },
            "shows chore details",
            async tokens =>
            {
                Guid id = Guid.Parse(tokens[1]);
                var dto = await _svc.GetById(id);
                await Console.Out.ShowDetailsBrief(id, dto);
            });

        private static readonly Command FinishChoreCommand = new Command(
            "FinishChore",
            new [] {"finish"},
            new[]
            {
                new Parameter("chore id", Parameter.IntParses),
            },
            "finishes a chore",
            async tokens =>
            {
                Guid id = Guid.Parse(tokens[1]);
                Guid? newid = await _svc.Finish(id);
                var dto = await _svc.GetById(id);
                await Console.Out.ShowDetailsBrief(id, dto);
                if (newid.HasValue)
                {
                    await Console.Out.WriteLineAsync("A new recurrence has been created");
                    dto = await _svc.GetById(newid.Value);
                    await Console.Out.ShowDetailsBrief(newid.Value, dto);
                }
            });

        internal static void Main()
        {
            //// IChoreRepository RepoBuilder() => new DapperChoreRepository(ConnectionStrings.ByName("Trolo"));
            //// IChoreRepository RepoBuilder() => InMemoryChoreRepository.WithStaticStorage();
            IChoreRepository RepoBuilder() => new AzureChoreRepository();

            IChoreApplicationService basicSvc = new ChoreApplicationService(RepoBuilder, SystemClock.Instance);

            var timerExpirations = new Subject<TimeDto>();
            var timerStarts = new Subject<TimeDto>();
            var sched = new ChoreApplicationServiceSchedulingAdapter(basicSvc, TaskPoolScheduler.Default,
                timerStarts,
                timerExpirations);

            timerStarts
                .Select(dto => dto.ToString())
                .Subscribe(x => Console.WriteLine($"Timer started, due {x}"));

            timerExpirations
                .Select(dto => dto.ToString())
                .Subscribe(x => Console.WriteLine($"Timer expired"));

            _svc = sched;

            var runner = new CommandRunner(
                CreateChoreCommand,
                ShowChoreCommand,
                FinishChoreCommand);

            CommandRunner.QuitCommand.Verbs = new List<string> {"quit"};
            runner.Run().GetAwaiter().GetResult();
        }
    }
}
