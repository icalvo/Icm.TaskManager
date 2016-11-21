using System;
using System.Linq;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        public abstract bool Matches(string line);

        public void Process(IObserver<string> output, string line)
        {
            Process(output, CommandLineTokenizer.Tokenize(line).ToArray());
        }

        protected abstract void Process(IObserver<string> output, string[] tokens);
    }
}