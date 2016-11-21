using System;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class QuitCommand : ICommand
    {
        public bool Matches(string line)
        {
            return line.StartsWith("quit");
        }

        public void Process(IObserver<string> output, string line)
        {
        }
    }
}