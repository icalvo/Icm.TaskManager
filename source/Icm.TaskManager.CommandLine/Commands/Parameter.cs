using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class Parameter
    {
        public Parameter(string name)
            : this(name, _ => Enumerable.Empty<string>())
        {
        }

        public Parameter(string name, Func<string, bool> valid)
        {
            Name = name;
            Validation = arg =>
                valid(arg)
                    ? Enumerable.Empty<string>()
                    : new[] { $"{name} is invalid" };
        }

        public Parameter(string name, Func<string, bool> valid, Func<string, string> errorMessage)
        {
            Name = name;
            Validation = arg =>
                valid(arg)
                    ? Enumerable.Empty<string>()
                    : new[] {errorMessage(arg)};
        }

        private Parameter(string name, Func<string, IEnumerable<string>> validation)
        {
            Name = name;
            Validation = validation;
        }

        public string Name { get; }
        public Func<string, IEnumerable<string>> Validation { get; }

        public static bool DateParses(string arg, string format)
        {
            return DateTime.TryParseExact(
                arg,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime d);
        }

        public static bool IntParses(string arg)
        {
            return int.TryParse(arg, out int i);
        }
    }
}