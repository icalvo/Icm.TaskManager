using FluentAssertions;
using Xunit;

namespace Icm.ChoreManager.Tests
{
    public class CommandLineTokenizerTests
    {
        [Fact]
        public void SingleUnquotedToken()
        {
            "acg".Should().ParseTo("acg");
        }

        [Fact]
        public void Multiple_Unquoted_WellFormatted()
        {
            "a bc d".Should().ParseTo("a", "bc", "d");
        }

        [Fact]
        public void Multiple_Unquoted_Untrimmed()
        {
            "   a bc  d    ".Should().ParseTo("a", "bc", "d");
        }

        [Fact]
        public void Multiple_Unquoted_Untrimmed_Backslash()
        {
            @"   a bc\  d    ".Should().ParseTo("a", @"bc\", "d");
        }

        [Fact]
        public void Multiple_Quoted_Untrimmed()
        {
            @"   ""a bc "" ""d""    ".Should().ParseTo("a bc ", "d");
        }

        [Fact]
        public void Multiple_Quoted_QuoteEscape_Untrimmed_EscapeQuotes()
        {
            @"   ""a bc \""test  \"" ""   ""d""    ".Should().ParseTo(@"a bc ""test  "" ", "d");
        }
    }
}