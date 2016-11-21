using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icm.TaskManager.Domain.Tests
{
    [TestClass]
    public class CommandLineTokenizerTests
    {
        [TestMethod]
        public void SingleUnquotedToken()
        {
            "acg".ShouldParseTo("acg");
        }

        [TestMethod]
        public void Multiple_Unquoted_WellFormatted()
        {
            "a bc d".ShouldParseTo("a", "bc", "d");
        }

        [TestMethod]
        public void Multiple_Unquoted_Untrimmed()
        {
            "   a bc  d    ".ShouldParseTo("a", "bc", "d");
        }

        [TestMethod]
        public void Multiple_Unquoted_Untrimmed_Backslash()
        {
            @"   a bc\  d    ".ShouldParseTo("a", @"bc\", "d");
        }

        [TestMethod]
        public void Multiple_Quoted_Untrimmed()
        {
            @"   ""a bc "" ""d""    ".ShouldParseTo("a bc ", "d");
        }

        [TestMethod]
        public void Multiple_Quoted_QuoteEscape_Untrimmed_EscapeQuotes()
        {
            @"   ""a bc \""test  \"" ""   ""d""    ".ShouldParseTo(@"a bc ""test  "" ", "d");
        }
    }
}