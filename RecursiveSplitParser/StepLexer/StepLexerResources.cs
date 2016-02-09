using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public static class StepLexerResources
    {
        public const string TerminalListEmpty = "Terminal list empty";
        public const string SourceLineEmpty = "Source line empty or whitespace: |{0}|";
        public const string SourceLinesListEmpty = "Source lines list empty";
        public const string LexerCustomEventArgs_SplitLexerPath = "SplitLexerPath";
        public const string LexerCustomEventArgs_CollapseLexerPath = "CollapseLexerPath";
        public const int LexerPathId_ALLPATHS = 0;
    }
}
