using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using Icm.TaskManager.CommandLine.Commands;

namespace Icm.ChoreManager.Tests
{
    internal static class StringCollectionAssertionsExtensions
    {
        public static AndConstraint<StringCollectionAssertions> ParseTo(
            this StringAssertions line, params string[] expected)
        {
            
            return CommandLineTokenizer.Tokenize(line.Subject).Should().BeEquivalentTo(expected, $"it's the expected parse of [{line}]");
        }
    }
}