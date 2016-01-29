using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecursiveSplitParser;
using System.Collections.ObjectModel;

namespace Lexer.UnitTests
{
    public class StepLexer_UnitTests
    {
        [Theory, StepLexerTestConvensions]
        public void CreateInstance_SetEmptyTerminalsThrowsException()
        {
            Exception expected = new Exception(StepLexerResources.TerminalListEmpty);
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(new ObservableCollection<Terminal>(), new ObservableCollection<string>(), new StepLexerOptions());
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
                StepLexer sut = new StepLexer(GetTestTerminalList(), new ObservableCollection<string>(), new StepLexerOptions());
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
                StepLexer sut = new StepLexer(GetTestTerminalList(), GetSingleTextLine(), new StepLexerOptions());
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
            StepLexer sut = new StepLexer(GetTestTerminalList(), GetSingleTextLine(), new StepLexerOptions());
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
            StepLexer sut = new StepLexer(GetTestTerminalListWithSpace(), GetSingleTextLine(), new StepLexerOptions());
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


        #region HelperFunctions
        private ObservableCollection<Terminal> GetTestTerminalList()
        {
            ObservableCollection<Terminal> terminals = new ObservableCollection<Terminal>();
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

        private ObservableCollection<Terminal> GetTestTerminalListWithSpace()
        {
            ObservableCollection<Terminal> terminals = new ObservableCollection<Terminal>();
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

        private ObservableCollection<string> GetSingleTextLine()
        {
            ObservableCollection<string> sourceLines = new ObservableCollection<string>()
            {
                " Lars gik en tur"
            };
            return sourceLines;
        }

    private ObservableCollection<string> GetMultiTextLines()
        {
            ObservableCollection<string> sourceLines = new ObservableCollection<string>()
            {
                "Lars og Rita gik en tur.",
                "De så en isbjørn",
                "Senere kørte de hjem"
            };
            return sourceLines;
        }
        #endregion
    }
}
