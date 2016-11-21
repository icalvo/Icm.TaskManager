using Icm.TaskManager.Application;
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
            var runner = new CommandRunner(
                new CreateTaskCommand(svc),
                new QuitCommand());

            runner.Run();
        }
    }
}
