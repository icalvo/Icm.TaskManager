using System;
using System.Collections.Generic;
using System.Linq;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class Command
    {
        public static Command WithoutVerb(string name, string help, Action<string[]> process)
        {
            return new Command(name, help, process);
        }

        private readonly Action<string[]> process;
        private readonly string help;
        private string v1;
        private string[] v2;
        private Parameter[] parameter;
        private string v3;
        private Action<string[]> p;

        public Command(
            string name,
            IEnumerable<string> verbs,
            IEnumerable<Parameter> parameters,
            string help,
            Action<string[]> process)
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
            Action<string[]> process)
            : this(name, new[] { verb }, parameters, help, process)
        {
        }

        public Command(
            string name,
            string verb,
            string help,
            Action<string[]> process)
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
            Action<string[]> process)
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
            Action<string[]> process)
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


        public void Process(string[] tokens)
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
                process(tokens);
            }
        }

        public IList<string> Verbs { get; set; }

        public IList<Parameter> Parameters { get; }
    }
}