using System;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal interface ICommand
    {
        bool Matches(string line);
        void Process(IObserver<string> output, string line);
    }
}