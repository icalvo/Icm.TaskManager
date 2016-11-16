using System;

namespace Icm.TaskManager.CommandLine
{
    internal interface ICommand
    {
        bool Matches(string line);
        void Process(IObserver<string> observer, string line);
    }
}