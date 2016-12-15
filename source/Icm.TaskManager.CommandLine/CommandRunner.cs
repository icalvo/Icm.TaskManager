using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
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


            this.commands = commands.Concat(new ICommand[]
            {
                new HelpCommand(commands),
                new QuitCommand()
            }).ToArray();
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
                .ToObservable()
                .SelectMany(cmd => cmd.command.Process(cmd.line))
                .Subscribe(Console.WriteLine);
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

    internal class HelpCommand : ICommand
    {
        public HelpCommand(params ICommand[] commands)
        {
            throw new NotImplementedException();
        }

        public bool Matches(IObserver<string> output, string line)
        {
            throw new NotImplementedException();
        }

        public IObservable<string> Process(string line)
        {
            throw new NotImplementedException();
        }
    }
}