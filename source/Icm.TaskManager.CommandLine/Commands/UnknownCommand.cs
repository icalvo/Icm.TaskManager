using System;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class UnknownCommand : ICommand
    {
        public bool Matches(IObserver<string> output, string line)
        {
            return false;
        }

        public void Process(IObserver<string> output, string line)
        {
            output.OnNext("Unknown command");
        }
    }
}