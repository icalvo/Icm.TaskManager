using Icm.TaskManager.Application;
using Icm.TaskManager.Infrastructure;
using NodaTime;

namespace Icm.TaskManager.CommandLine
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            // var repo = new DapperTaskRepository(ConnectionStrings.ByName("Trolo"));

            var repo = new InMemoryTaskRepository();
            var svc = new TaskApplicationService(repo, SystemClock.Instance);

            
        }
    }
}
