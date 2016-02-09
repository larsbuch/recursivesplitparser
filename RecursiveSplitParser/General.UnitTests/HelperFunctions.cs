using Moq;
using ReactiveUI;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.UnitTests
{
    public static class HelperFunctions
    {
        public static IGrammarContainer GetTestGrammar()
        {
            var grammarMock = new Mock<IGrammar>();
            IList<ITerminal> terminalList = new List<ITerminal>()
                {
                    GetTerminalMock("SPACE", " ", true)
                    , GetTerminalMock("LARS", "Lars", false)
                    , GetTerminalMock("RITA", "Rita", false)
                    , GetTerminalMock("WORD", "\\w+", false)
                };
            grammarMock.SetupGet(r => r.Terminals).Returns(terminalList);
            var grammarContainerMock = new Mock<IGrammarContainer>();
            grammarContainerMock.Setup(g => g.GetBaseGrammar()).Returns(grammarMock.Object);
            return grammarContainerMock.Object;
        }

        public static IGrammarContainer GetTestGrammarWithSpace()
        {
            var grammarMock = new Mock<IGrammar>();
            IList<ITerminal> terminalList = new List<ITerminal>()
                {
                    GetTerminalMock("SPACE", " ", false)
                    , GetTerminalMock("LARS", "Lars", false)
                    , GetTerminalMock("RITA", "Rita", false)
                    , GetTerminalMock("WORD", "\\w+", false)
                };
            grammarMock.SetupGet(r => r.Terminals).Returns(terminalList);
            var grammarContainerMock = new Mock<IGrammarContainer>();
            grammarContainerMock.Setup(g => g.GetBaseGrammar()).Returns(grammarMock.Object);
            return grammarContainerMock.Object;
        }

        public static ITerminal GetTerminalMock(string terminalName, string terminalMatch, bool ignoreTerminal)
        {
            var terminalMock = new Mock<ITerminal>();
            terminalMock.Setup(t => t.IgnoreTerminal).Returns(ignoreTerminal);
            terminalMock.Setup(u => u.TerminalName).Returns(terminalName);
            terminalMock.Setup(v => v.TerminalMatch).Returns(terminalMatch);
            return terminalMock.Object;
        }

        public static ReactiveList<SourceCodeLine> GetSingleTextLine()
        {
            return GetSourceCodeLines(new[]
                {
                    " Lars gik en tur"
                });
        }

        public static ReactiveList<SourceCodeLine> GetMultiTextLines()
        {
            return GetSourceCodeLines(new[]
                {
                    "Lars og Rita gik en tur.",
                    "De så en isbjørn",
                    "Senere kørte de hjem"
                });
        }

        public static ReactiveList<SourceCodeLine> GetSourceCodeLines(string[] lines)
        {
            ReactiveList<SourceCodeLine> sourceCodeLines = new ReactiveList<SourceCodeLine>();
            foreach (string line in lines)
            {
                sourceCodeLines.Add(new SourceCodeLine(line));
            }

            return sourceCodeLines;
        }
    }
}
