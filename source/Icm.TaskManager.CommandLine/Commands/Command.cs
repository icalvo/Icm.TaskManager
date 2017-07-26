using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icm.ChoreManager.CommandLine.Commands
{
    internal class Command
    {
        public static Command Create(
            string name,
            string verb,
            string help,
            Func<Task> process)
        {
            return new Command(
                name,
                verb,
                new IParameter[0],
                help,
                arr => process());
        }

        public static Command Create<T1>(
            string name,
            string verb,
            Parameter<T1> parameter1,
            string help,
            Func<T1, Task> process)
        {
            return new Command(
                name,
                verb,
                new IParameter[] { parameter1 },
                help,
                arr => process((T1)arr[0]));
        }

        public static Command Create<T1, T2>(
            string name,
            string verb,
            Parameter<T1> parameter1,
            Parameter<T2> parameter2,
            string help,
            Func<T1, T2, Task> process)
        {
            return new Command(
                name,
                verb,
                new IParameter[] {parameter1, parameter2},
                help,
                arr => process((T1)arr[0], (T2)arr[1]));
        }

        public static Command WithoutVerb(string name, string help, Func<object[], Task> process)
        {
            return new Command(name, help, process);
        }

        private readonly Func<object[], Task> process;
        private readonly string help;

        public Command(
            string name,
            IEnumerable<string> verbs,
            IEnumerable<IParameter> parameters,
            string help,
            Func<object[], Task> process)
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
            IEnumerable<IParameter> parameters,
            string help,
            Func<object[], Task> process)
            : this(name, new[] { verb }, parameters, help, process)
        {
        }

        public Command(
            string name,
            string verb,
            string help,
            Func<object[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = new[] { verb };
            Parameters = new List<IParameter>();
        }

        public Command(
            string name,
            IEnumerable<string> verbs,
            string help,
            Func<object[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = verbs.ToList();
            Parameters = new List<IParameter>();
        }

        private Command(
            string name,
            string help,
            Func<object[], Task> process)
        {
            Name = name;
            this.process = process;
            this.help = help;
            Verbs = new List<string>();
            Parameters = new List<IParameter>();
        }

        public bool Matches(string[] tokens)
        {
            return Verbs.Any(tokens.VerbIs);
        }

        private IEnumerable<ICommandParseResult> Results(string[] tokens)
        {
            if (!Parameters.Any())
            {
                yield break;
            }

            if (Parameters.Any() && tokens.Length != Parameters.Count + 1)
            {
                yield return CommandParseResult.GeneralError($"I need {Parameters.Count} arguments ({Parameters.JoinStr(", ", p => p.Name)}) to execute {Name}");
                yield break;
            }

            var results = 
                tokens
                    .Skip(1)
                    .Zip(Parameters, (arg, param) => param.Parse(arg));
            foreach (var error in results)
            {
                yield return error;
            }
        }

        public string Name { get; }

        public string Help => $"{help}\r\nUsage: {Verbs.JoinStr("/")} {Parameters.JoinStr(" ", p => p.Name.Replace(" ", "_"))}\r\n";


        public async Task Process(string[] tokens)
        {
            var results = Results(tokens).ToList();

            var errors = results.Where(x => !x.IsValid).ToList();
            if (errors.Any())
            {
                foreach (var result in errors)
                {
                    Console.Error.WriteLine(result);
                }
            }
            else
            {
                await Console.Out.WriteLineAsync($"Executing {Name}...");
                await process(results.Select(x => x.Value).ToArray());
            }
        }

        public IList<string> Verbs { get; set; }

        public IList<IParameter> Parameters { get; }
    }
}