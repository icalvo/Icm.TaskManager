using System;

namespace Icm.ChoreManager.CommandLine.Commands
{
    internal class Parameter<T> : IParameter
    {
        private readonly Func<string, CommandParseResult<T>> parseFunc;

        public Parameter(string name, Func<string, CommandParseResult<T>> parseFunc)
        {
            this.parseFunc = parseFunc;
            Name = name;
        }

        public string Name { get; }

        public ICommandParseResult Parse(string token)
        {
            return parseFunc(token);
        }
    }
}