using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icm.ChoreManager.CommandLine.Commands
{
    internal class Command
    {
        public static Command WithoutVerb(string name, string help, Func<string[], Task> process)
        {
            return new Command(name, help, process);
        }

        private readonly Func<string[], Task> process;
        private readonly string help;

        public Command(
            string name,
            IEnumerable<string> verbs,
            IEnumerable<Parameter> parameters,
            string help,
            Func<string[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = verbs.ToList();
            Parameters = parameters.ToList();
        }

        public Command(
            string name,
            string verb,
            IEnumerable<Parameter> parameters,
            string help,
            Func<string[], Task> process)
            : this(name, new[] { verb }, parameters, help, process)
        {
        }

        public Command(
            string name,
            string verb,
            string help,
            Func<string[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = new[] { verb };
            Parameters = new List<Parameter>();
        }

        public Command(
            string name,
            IEnumerable<string> verbs,
            string help,
            Func<string[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = verbs.ToList();
            Parameters = new List<Parameter>();
        }

        private Command(
            string name,
            string help,
            Func<string[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = new List<string>();
            Parameters = new List<Parameter>();
        }

        public bool Matches(string[] tokens)
        {
            return Verbs.Any(tokens.VerbIs);
        }

        private IEnumerable<string> ArgumentErrors(string[] tokens)
        {
            if (!Parameters.Any())
            {
                yield break;
            }

            if (Parameters.Any() && tokens.Length != Parameters.Count + 1)
            {
                yield return $"I need {Parameters.Count} arguments ({Parameters.JoinStr(", ", p => p.Name)}) to execute {Name}";
                yield break;
            }

            var errors = 
                tokens
                    .Skip(1)
                    .Zip(Parameters, (arg, param) => param.Validation(arg))
                    .SelectMany(x => x);
            foreach (var error in errors)
            {
                yield return error;
            }
        }

        public string Name { get; }

        public string Help => $"{help}\r\nUsage: {Verbs.JoinStr("/")} {Parameters.JoinStr(" ", p => p.Name.Replace(" ", "_"))}\r\n";


        public async Task Process(string[] tokens)
        {
            var errors = ArgumentErrors(tokens).ToArray();
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    Console.Error.WriteLine(error);
                }
            }
            else
            {
                await process(tokens);
            }
        }

        public IList<string> Verbs { get; set; }

        public IList<Parameter> Parameters { get; }
    }
}