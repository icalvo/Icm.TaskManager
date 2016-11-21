using System;
using System.Text;

namespace Icm.TaskManager.CommandLine
{
    internal interface ICommand
    {
        bool Matches(string line);
        void Process(IObserver<string> output, string line);
    }
}