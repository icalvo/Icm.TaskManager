using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icm.TaskManager.Domain.Tests
{
    [TestClass]
    public class TokenizerTests
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
        public void Multiple_Unquoted_Untrimmed_EscapeSpace()
        {
            @"   a bc\  d    ".ShouldParseTo("a", @"bc\", "d");
        }

        [TestMethod]
        public void Multiple_Quoted_Untrimmed_EscapeSpace()
        {
            @"   ""a bc "" ""d""    ".ShouldParseTo("a bc ", "d");
        }

        [TestMethod]
        public void Multiple_Quoted_QuoteEscape_Untrimmed_EscapeSpace()
        {
            @"   ""a bc \""test  \"" ""   ""d""    ".ShouldParseTo(@"a bc ""test  "" ", "d");
        }
    }
}