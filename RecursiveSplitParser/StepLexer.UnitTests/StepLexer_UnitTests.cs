using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecursiveSplitParser;
using System.Collections.ObjectModel;
using ReactiveUI;
using Moq;
using General.UnitTests;

namespace Lexer.UnitTests
{
    public class StepLexer_UnitTests
    {
        [Theory, StepLexerTestConvensions]
        public void CreateInstance_SetTerminalsThrowsNoException(IParser parser)
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                StepLexer sut = new StepLexer(parser, HelperFunctions.GetTestGrammar(), HelperFunctions.GetSingleTextLine(), new StepLexerOptions());
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
            TestParser testParser = new TestParser();
            StepLexer sut = new StepLexer(testParser, HelperFunctions.GetTestGrammar(), HelperFunctions.GetSingleTextLine(), new StepLexerOptions());
            testParser.Parse();
            Assert.Equal(8, testParser.Tokens.Count);
            Assert.Equal("BOF", testParser.Tokens[0].Terminal);
            Assert.Equal("BOL", testParser.Tokens[1].Terminal);
            Assert.Equal("LARS", testParser.Tokens[2].Terminal);
            Assert.Equal("WORD", testParser.Tokens[3].Terminal);
            Assert.Equal("WORD", testParser.Tokens[4].Terminal);
            Assert.Equal("WORD", testParser.Tokens[5].Terminal);
            Assert.Equal("EOL", testParser.Tokens[6].Terminal);
            Assert.Equal("EOF", testParser.Tokens[7].Terminal);
        }

        //[Theory, StepLexerTestConvensions]
        //public void NextToken_ReturnsCorrectListOfTokensWithSpaceWithoutIgnore()
        //{
        //    StepLexer sut = new StepLexer(GetTestTerminalListWithSpace(), GetSingleTextLine(), new StepLexerOptions());
        //    Token token = sut.NextToken();
        //    Assert.Equal("BOF", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("BOL", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("SPACE", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("LARS", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("SPACE", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("WORD", token.Terminal);
        //    Assert.Equal("gik", token.Match);
        //    token = sut.NextToken();
        //    Assert.Equal("SPACE", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("WORD", token.Terminal);
        //    Assert.Equal("en", token.Match);
        //    token = sut.NextToken();
        //    Assert.Equal("SPACE", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("WORD", token.Terminal);
        //    Assert.Equal("tur", token.Match);
        //    token = sut.NextToken();
        //    Assert.Equal("EOL", token.Terminal);
        //    token = sut.NextToken();
        //    Assert.Equal("EOF", token.Terminal);
        //}
    }
}
