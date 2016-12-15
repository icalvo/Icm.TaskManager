using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Icm.TaskManager.Application;
using Icm.TaskManager.CommandLine.Commands;
using Icm.TaskManager.Infrastructure;
using NodaTime;

namespace Icm.TaskManager.CommandLine
{
    internal static class Program
    {

        internal static void Main()
        {
            // var repo = new DapperTaskRepository(ConnectionStrings.ByName("Trolo"));

            var repo = new InMemoryTaskRepository();
            ITaskApplicationService svc = new TaskApplicationService(repo, SystemClock.Instance);

            var scheduler = new TaskApplicationServiceSchedulingAdapter(svc, TaskPoolScheduler.Default);

            scheduler.TimeChanges
                .Select(dto => dto.ToString())
                .Subscribe(Console.WriteLine);

            var runner = new CommandRunner(
                new CreateTaskCommand(svc),
                new ShowTaskCommand(svc));

            runner.Run();
        }
    }
}
