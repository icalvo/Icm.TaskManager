using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Subjects;
using Icm.TaskManager.CommandLine.Commands;

namespace Icm.TaskManager.CommandLine
{
    internal class CommandRunner
    {
        private readonly ICommand[] commands;
        private readonly ICommand unknownCommand = new UnknownCommand();

        public CommandRunner(params ICommand[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            if (!commands.Any(cmd => cmd is QuitCommand))
            {
                throw new ArgumentException("There's no QuitCommand, you won't be able to exit the command application.");
            }

            this.commands = commands;
        }

        public void Run()
        {
            var o = new Subject<string>();
            o.Subscribe(Console.WriteLine);
            ConsoleInput()
                .Select(line =>
                {
                    return new { line, command = commands.FirstOrDefault(cmd => cmd.Matches(o, line)) ?? unknownCommand };
                })
                .TakeWhile(cmd => !(cmd.command is QuitCommand))
                .Execute(cmd =>
                {
                    cmd.command.Process(o, cmd.line);
                });
        }

        [SuppressMessage("ReSharper", "IteratorNeverReturns",
             Justification = "It is really an infinite sequence.")]
        private static IEnumerable<string> ConsoleInput()
        {
            while (true)
            {
                Console.Write("> ");
                yield return Console.ReadLine();
            }
        }
    }
}