using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepLexer
{
    public class Token
    {
        public const string NULL = "NULL";
        public const string BOF = "BOF";
        public const string BOL = "BOL";
        public const string EOL = "EOL";
        public const string EOF = "EOF";
        public const string INDENT_INCREASED = "INDENT_INCREASED";
        public const string INDENT_DECREASED = "INDENT_DECREASED";
        public const string UNKNOWN_TERMINAL = "UNKNOWN_TERMINAL";
        public const int LINEPOSITION_START = 0;
        public const int CHARPOSITION_START = 0;
        public const int INDENT_START = 0;

        public string Match { get; private set; }

        public string Terminal { get; private set; }

        public int LinePosition { get; private set; }

        public int CharPosition { get; private set; }

        public int LexerPathId { get; set; }

        public static Token NewNullToken
        {
            get
            {
                return new Token(LexerPath.NOTSET, Token.NULL, string.Empty, Token.LINEPOSITION_START, Token.CHARPOSITION_START);
            }
        }

        public Token(int lexerPathId, string terminal, string match, int linePosition, int charPosition)
        {
            LexerPathId = lexerPathId;
            Match = match;
            Terminal = terminal;
            LinePosition = linePosition;
            CharPosition = charPosition;
        }

        public override string ToString()
        {
            return Terminal + ": " + Match + "(" + LinePosition + "," + CharPosition + ")";
        }

        public Token Clone()
        {
            return new Token(LexerPath.NOTSET, Terminal, Match, LinePosition, CharPosition);
        }
    }
}
