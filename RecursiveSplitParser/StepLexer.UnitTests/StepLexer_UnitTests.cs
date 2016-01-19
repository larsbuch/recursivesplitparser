using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecursiveSplitParser;

namespace StepLexer.UnitTests
{
    public class StepLexer_UnitTests
    {
        [Theory, StepLexerTestConvensions]
        public void CreateInstance_SetEmptyTerminalsThrowsException()
        {
            List<Terminal> terminals = new List<Terminal>();

            Exception expected = new Exception(StepLexerResources.TerminalListEmpty);
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(terminals);
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected.Message, actual.Message);
        }

        [Theory, StepLexerTestConvensions]
        public void CreateInstance_SetTerminalsThrowsNoException()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(GetTestTerminalList());
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, StepLexerTestConvensions]
        public void SetSourceLine_ThrowsNoException()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(GetTestTerminalList());
                sut.SetSourceLine(GetSingleTextLine());
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, StepLexerTestConvensions]
        public void NextToken_ReturnsCorrectListOfTokens()
        {
            StepLexer sut = new StepLexer(GetTestTerminalList());
            sut.SetSourceLine(GetSingleTextLine());
            Token token = sut.NextToken();
            Assert.Equal("BOF", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("BOL", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("LARS", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("WORD", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("WORD", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("WORD", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("EOL", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("EOF", token.Terminal);
        }

        [Theory, StepLexerTestConvensions]
        public void NextToken_ReturnsCorrectListOfTokensWithSpaceWithoutIgnore()
        {
            StepLexer sut = new StepLexer(GetTestTerminalListWithSpace());
            sut.SetSourceLine(GetSingleTextLine());
            Token token = sut.NextToken();
            Assert.Equal("BOF", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("BOL", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("SPACE", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("LARS", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("SPACE", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("WORD", token.Terminal);
            Assert.Equal("gik", token.Match);
            token = sut.NextToken();
            Assert.Equal("SPACE", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("WORD", token.Terminal);
            Assert.Equal("en", token.Match);
            token = sut.NextToken();
            Assert.Equal("SPACE", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("WORD", token.Terminal);
            Assert.Equal("tur", token.Match);
            token = sut.NextToken();
            Assert.Equal("EOL", token.Terminal);
            token = sut.NextToken();
            Assert.Equal("EOF", token.Terminal);
        }

        [Theory, StepLexerTestConvensions]
        public void SetSourceLine_EmptyStringThrowsNoException()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(GetTestTerminalList());
                sut.SetSourceLine(string.Empty);
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, StepLexerTestConvensions]
        public void SetSourceLines_ThrowsNoException()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(GetTestTerminalList());
                sut.SetSourceLines(GetMultiTextLines());
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, StepLexerTestConvensions]
        public void SetSourceLines_EmptyListThrowsNoException()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(GetTestTerminalList());
                sut.SetSourceLines(new List<string>());
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);

        }

        #region HelperFunctions
        private List<Terminal> GetTestTerminalList()
        {
            List<Terminal> terminals = new List<Terminal>();
            // space match (and ignore in token list)
            terminals.Add(new Terminal("SPACE", " ", true));
            // Lars match
            terminals.Add(new Terminal("LARS", "Lars", false));
            // Rita match
            terminals.Add(new Terminal("RITA", "Rita", false));
            // Match words
            terminals.Add(new Terminal("WORD", "\\w+", false));
            return terminals;
        }

        private List<Terminal> GetTestTerminalListWithSpace()
        {
            List<Terminal> terminals = new List<Terminal>();
            // space match (and ignore in token list)
            terminals.Add(new Terminal("SPACE", " ", false));
            // Lars match
            terminals.Add(new Terminal("LARS", "Lars", false));
            // Rita match
            terminals.Add(new Terminal("RITA", "Rita", false));
            // Match words
            terminals.Add(new Terminal("WORD", "\\w+", false));
            return terminals;
        }

        private string GetSingleTextLine()
        {
            return " Lars gik en tur";
        }

        private List<string> GetMultiTextLines()
        {
            List<string> textLines = new List<string>()
            {
                "Lars og Rita gik en tur.",
                "De så en isbjørn",
                "Senere kørte de hjem"
            };
            return textLines;
        }
        #endregion
    }
}
