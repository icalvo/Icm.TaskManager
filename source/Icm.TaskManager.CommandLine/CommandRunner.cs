using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Icm.TaskManager.CommandLine.Commands;
using JetBrains.Annotations;
using static System.Threading.Tasks.Task;

namespace Icm.TaskManager.CommandLine
{
    internal class CommandRunner
    {
        private IEnumerable<Command> commands;
        private static readonly Command UnknownCommand = Command.WithoutVerb(
            "UnknownCommand",
            "Unknown command",
            async _ => await Console.Error.WriteLineAsync("Unknown command!"));

        private static readonly Command NullCommand = Command.WithoutVerb(
            "NullCommand",
            "Null command",
            _ => CompletedTask);

        public static readonly Command QuitCommand = new Command(
            "QuitApplication",
            new[] { "quit", "q", "exit" },
            "quits the manager",
            _ => CompletedTask);

        public CommandRunner(params Command[] commands)
        {
            var helpCommand = new Command(
                "ShowHelp",
                "help",
                "shows help",
                async tokens =>
                {
                    foreach (var command in this.commands)
                    {
                        await Console.Out.WriteLineAsync(command.Help);
                    }
                });

            Commands = commands.Concat(new[]
            {
                helpCommand,
                QuitCommand
            }).ToArray();
        }

        public IEnumerable<Command> Commands
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(commands));
                }

                Validate(value);
                commands = value;
            }
        }

        private void Validate(IEnumerable<Command> value)
        {
            var query =
                from command in value
                from verb in command.Verbs
                group command by verb
                into g
                where g.Count() > 1
                select g;

            if (query.Any())
            {
                var groupsOutput = query.JoinStr("\r\n", g =>
                {
                    var commandsWithDuplicateVerb = g.JoinStr(", ", command => command.Name);
                    return $"Verb {g.Key} is shared by commands {commandsWithDuplicateVerb}";
                });
                throw new ArgumentException($"There are duplicate verbs: {groupsOutput}", nameof(commands));
            }
        }

        public Task Run()
        {
            return 
                ConsoleInput()
                .Select(line =>
                {
                    var tokens = CommandLineTokenizer.Tokenize(line).ToArray();
                    var result = TokensToCommand(tokens);

                    return new { tokens, command = result };
                })
                .TakeWhile(cmd => cmd.command != QuitCommand)
                .ExecuteAsync(async cmd =>
                {
                    try
                    {
                        await cmd.command.Process(cmd.tokens);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"ERROR: {ex.Message}");
                        Console.Out.WriteLine("Use the 'stack' command to see the stack trace.");
                    }
                });
        }

        [NotNull]
        private Command TokensToCommand(string[] tokens)
        {
            var result = UnknownCommand;
            if (!tokens.Any())
            {
                result = NullCommand;
            }
            else
            {
                var firstMatch = commands.FirstOrDefault(cmd => cmd.Matches(tokens));
                if (firstMatch != null)
                {
                    result = firstMatch;
                }
            }
            return result;
        }

        [SuppressMessage("ReSharper", "IteratorNeverReturns",
             Justification = "It is really an infinite sequence.")]
        [NotNull]
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