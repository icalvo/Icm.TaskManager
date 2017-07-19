using Xunit;

namespace Icm.TaskManager.Tests
{
    public class CommandLineTokenizerTests
    {
        [Fact]
        public void SingleUnquotedToken()
        {
            "acg".ShouldParseTo("acg");
        }

        [Fact]
        public void Multiple_Unquoted_WellFormatted()
        {
            "a bc d".ShouldParseTo("a", "bc", "d");
        }

        [Fact]
        public void Multiple_Unquoted_Untrimmed()
        {
            "   a bc  d    ".ShouldParseTo("a", "bc", "d");
        }

        [Fact]
        public void Multiple_Unquoted_Untrimmed_Backslash()
        {
            @"   a bc\  d    ".ShouldParseTo("a", @"bc\", "d");
        }

        [Fact]
        public void Multiple_Quoted_Untrimmed()
        {
            @"   ""a bc "" ""d""    ".ShouldParseTo("a bc ", "d");
        }

        [Fact]
        public void Multiple_Quoted_QuoteEscape_Untrimmed_EscapeQuotes()
        {
            @"   ""a bc \""test  \"" ""   ""d""    ".ShouldParseTo(@"a bc ""test  "" ", "d");
        }
    }
}