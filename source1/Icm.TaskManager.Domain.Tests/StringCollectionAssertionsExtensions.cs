using System.Reactive.Subjects;
using FluentAssertions;
using FluentAssertions.Collections;
using Icm.TaskManager.CommandLine.Commands;

namespace Icm.TaskManager.Domain.Tests
{
    internal static class StringCollectionAssertionsExtensions
    {
        public static AndConstraint<StringCollectionAssertions> ShouldParseTo(
            this string line, params string[] expected)
        {
            var output = new Subject<string>();
            return CommandLineTokenizer.Tokenize(line).Should().BeEquivalentTo(expected, $"it's the expected parse of [{line}]");
        }
    }
}