using System;

namespace StepLexer
{
    public class CheckNextTokenEventArgs:EventArgs
    {
        public const int ALLPATHS = 0;
        public int LexerPathId { get; set; }
    }
}