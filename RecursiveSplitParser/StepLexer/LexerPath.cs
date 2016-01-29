using Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
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
        internal RecursiveGrammar ActiveGrammar { get; set; }
        public static LexerPath StartLexerPath
        {
            get
            {
                return new LexerPath(LexerPath.NOTSET, GrammarContainer.BaseGrammar, Token.LINEPOSITION_START, Token.CHARPOSITION_START, Token.INDENT_START, Token.NewNullToken);
            }
        }

        internal LexerPath(int parentLexerPathID, RecursiveGrammar activeGrammar, int activeLineNumber, int activeCharacterNumber, int activeIndentNumber, Token currentToken):this(LexerPath.NOTSET, parentLexerPathID, activeGrammar, activeLineNumber, activeCharacterNumber, activeIndentNumber, currentToken)
        {
        }

        internal LexerPath(int lexerPathID, int parentLexerPathID, RecursiveGrammar activeGrammar, int activeLineNumber, int activeCharacterNumber, int activeIndentNumber, Token currentToken)
        {
            LexerPathID = lexerPathID;
            ParentLexerPathID = parentLexerPathID;
            ActiveGrammar = activeGrammar;
            ActiveLineNumber = activeLineNumber;
            ActiveCharacterNumber = activeCharacterNumber;
            ActiveIndentNumber = activeIndentNumber;
            CurrentToken = currentToken;
        }

        internal LexerPath Clone()
        {
            return new LexerPath(LexerPathID, ActiveGrammar , ActiveLineNumber, ActiveCharacterNumber, ActiveIndentNumber, CurrentToken.Clone());
        }
    }
}
