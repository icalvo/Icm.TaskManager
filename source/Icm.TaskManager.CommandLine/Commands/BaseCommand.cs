using System;
using System.Linq;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        private string[] tokens;

        public bool Matches(IObserver<string> output, string line)
        {
            tokens = CommandLineTokenizer.Tokenize(line, output).ToArray();
            return Matches(tokens);
        }

        protected abstract bool Matches(string[] tokens);

        protected virtual bool Validates(IObserver<string> output, string[] tokens)
        {
            return true;
        }

        public void Process(IObserver<string> output, string line)
        {
            if (Validates(output, tokens))
            {
                Process(output, tokens);
            }
        }

        protected abstract void Process(IObserver<string> output, string[] tokens);
    }
}