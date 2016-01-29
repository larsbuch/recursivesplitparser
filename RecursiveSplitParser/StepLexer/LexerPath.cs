using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepLexer
{
    public class LexerPath
    {
        internal const int NOTSET = 0;

        internal int LexerPathID { get; set; }
        internal int ParentLexerPathID { get; set; }
        internal int ActiveLineNumber { get; set; }
        internal int ActiveCharacterNumber { get; set; }
        internal int ActiveIndentNumber { get; set; }
        internal Token CurrentToken { get; set; }
        public static LexerPath StartLexerPath
        {
            get
            {
                return new LexerPath(LexerPath.NOTSET, Token.LINEPOSITION_START, Token.CHARPOSITION_START, Token.INDENT_START, Token.NewNullToken);
            }
        }

        internal LexerPath(int parentLexerPathID, int activeLineNumber, int activeCharacterNumber, int activeIndentNumber, Token currentToken):this(LexerPath.NOTSET, parentLexerPathID, activeLineNumber, activeCharacterNumber, activeIndentNumber, currentToken)
        {
        }

        internal LexerPath(int lexerPathID, int parentLexerPathID, int activeLineNumber, int activeCharacterNumber, int activeIndentNumber, Token currentToken)
        {
            LexerPathID = lexerPathID;
            ParentLexerPathID = parentLexerPathID;
            ActiveLineNumber = activeLineNumber;
            ActiveCharacterNumber = activeCharacterNumber;
            ActiveIndentNumber = activeIndentNumber;
            CurrentToken = currentToken;
        }

        internal LexerPath Clone()
        {
            return new LexerPath(LexerPathID, ActiveLineNumber, ActiveCharacterNumber, ActiveIndentNumber, CurrentToken.Clone());
        }
    }
}
