using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Icm.TaskManager.Application;
using Icm.TaskManager.CommandLine.Commands;
using Icm.TaskManager.Infrastructure;
using NodaTime;
using NodaTime.Text;

namespace Icm.TaskManager.CommandLine
{
    internal static class Program
    {
        private static ITaskApplicationService _svc;

        private static readonly Command CreateTaskCommand = new Command(
            "CreateTask",
            "create",
            new[] {
                new Parameter("description"),
                new Parameter("due date", arg => Parameter.DateParses(arg, "yyyy-MM-dd")) },
            "creates a task",
            tokens =>
            {
                Console.Out.WriteLine("Creating task...");

                var description = tokens[1];
                var dueDate = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd", DateTimeZoneProviders.Tzdb).Parse(tokens[2]).Value.ToInstant();
                var id = _svc.CreateTask(description, dueDate);
                Console.Out.WriteLine($"Task {id} created");
                var dto = _svc.GetTaskById(id);
                Console.Out.ShowDetailsBrief(id, dto);
            });

        private static readonly Command ShowTaskCommand = new Command(
            "ShowTask",
            "show",
            new []
            {
                new Parameter("task id", Parameter.IntParses), 
            },
            "shows task details",
            tokens =>
            {
                int id = int.Parse(tokens[1]);
                var dto = _svc.GetTaskById(id);
                Console.Out.ShowDetailsBrief(id, dto);
            });

        private static readonly Command FinishTaskCommand = new Command(
            "FinishTask",
            new [] {"finish"},
            new[]
            {
                new Parameter("task id", Parameter.IntParses),
            },
            "finish task",
            tokens =>
            {
                int id = int.Parse(tokens[1]);
                int? newid = _svc.FinishTask(id);
                var dto = _svc.GetTaskById(id);
                Console.Out.ShowDetailsBrief(id, dto);
                if (newid.HasValue)
                {
                    Console.Out.WriteLine("A new recurrence has been created");
                    dto = _svc.GetTaskById(newid.Value);
                    Console.Out.ShowDetailsBrief(newid.Value, dto);
                }
            });

        internal static void Main()
        {
            // var repo = new DapperTaskRepository(ConnectionStrings.ByName("Trolo"));

            var repo = new InMemoryTaskRepository();
            ITaskApplicationService basicSvc = new TaskApplicationService(repo, SystemClock.Instance);

            var sched = new TaskApplicationServiceSchedulingAdapter(basicSvc, TaskPoolScheduler.Default);

            sched.TimerStarts
                .Select(dto => dto.ToString())
                .Subscribe(x => Console.WriteLine($"Timer started, due {x}"));

            sched.TimerExpirations
                .Select(dto => dto.ToString())
                .Subscribe(x => Console.WriteLine($"Timer expired"));

            _svc = sched;

            var runner = new CommandRunner(
                CreateTaskCommand,
                ShowTaskCommand,
                FinishTaskCommand);

            CommandRunner.QuitCommand.Verbs = new List<string> {"quit"};
            runner.Run();
        }
    }
}
