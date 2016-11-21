using System;

namespace Icm.TaskManager.CommandLine
{
    internal class UnknownCommand : ICommand
    {
        public bool Matches(string line)
        {
            return false;
        }

        public void Process(IObserver<string> output, string line)
        {
            output.OnNext("Unknown command");
        }
    }
}