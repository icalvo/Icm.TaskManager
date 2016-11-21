using System;
using System.Linq;
using Icm.TaskManager.CommandLine.Commands;

namespace Icm.TaskManager.CommandLine
{
    internal abstract class BaseCommand : ICommand
    {
        public abstract bool Matches(string line);

        public void Process(IObserver<string> output, string line)
        {
            Process(output, Tokenizer.Tokenize(line).ToArray());
        }

        protected abstract void Process(IObserver<string> output, string[] tokens);
    }
}