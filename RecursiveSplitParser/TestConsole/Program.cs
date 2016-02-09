using General.UnitTests;
using Lexer;
using Microsoft.Reactive.Testing;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new TestScheduler();

            scheduler.

            TestParser testParser = new TestParser();
            StepLexer sut = new StepLexer(testParser, HelperFunctions.GetTestGrammar(), HelperFunctions.GetSingleTextLine(), new StepLexerOptions());



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

    }
}
