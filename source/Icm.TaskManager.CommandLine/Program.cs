using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Icm.TaskManager.Application;
using Icm.TaskManager.Domain.Tasks;
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
            ITaskApplicationService svc = new TaskApplicationService(repo, SystemClock.Instance);

            IClock clock = SystemClock.Instance;

            ICommand[] commands =
            {
                new CreateTaskCommand(svc), 
                new QuitCommand()
            };
            ConsoleInput()
                .Select(x => new {line = x, command = commands.FirstOrDefault(cmd => cmd.Matches(x))})
                .TakeWhile(cmd => !(cmd.command is QuitCommand))
                .Execute(cmd =>
                {
                    var o = new Subject<string>();
                    o.Subscribe(Console.WriteLine);
                    if (cmd.command == null)
                    {
                        o.OnNext("Unknown command");
                    }
                    else
                    {
                        cmd.command.Process(o, cmd.line);
                    }
                });
        }

        private static IEnumerable<string> ConsoleInput()
        {
            while (true)
            {
                Console.Write("> ");
                yield return Console.ReadLine();
            }
        }

        private static void Details(TaskId taskId, Task task)
        {
            Console.WriteLine($"Task {taskId}: {task.Description}");
            Console.WriteLine($"Due: {task.DueDate}");
            Console.WriteLine($"Priority: {task.Priority}");
            Console.WriteLine($"Labels: {task.Labels}");
            Console.WriteLine($"Notes: {task.Notes}");
        }
    }
}
