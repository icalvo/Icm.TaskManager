using FluentAssertions;
using FluentAssertions.Collections;
using Icm.TaskManager.CommandLine;
using Icm.TaskManager.CommandLine.Commands;

namespace Icm.TaskManager.Domain.Tests
{
    internal static class StringCollectionAssertionsExtensions
    {
        public static AndConstraint<StringCollectionAssertions> ShouldParseTo(
            this string line, params string[] expected)
        {
            return Tokenizer.Tokenize(line).Should().BeEquivalentTo(expected, $"it's the expected parse of [{line}]");
        }
    }
}