using NullGuard;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StepLexer
{
    /**
     * Responsible for tokenizing the input. Input is tokenized regex match done by NextToken() function. Using a stepwize 
     * lexer makes it possible to redefine terminals on the run making parsing Tex possible.
     * 
     * TODOs Handle indent change and terminal replacement and terminal ignore
     * 
     */
    public class StepLexer
    {
        private List<Terminal> _terminals;
        private Dictionary<int, string> _sourceLines;
        private Token _currentToken;
        private int _activeLineNumber = 0;
        private int _activeCharacterNumber = 0;
        private int _activeIndentNumber = 0;

        public StepLexer(List<Terminal> terminals)
        {
            GuardClauses.ListNotEmpty(terminals, StepLexerResources.TerminalListEmpty);
            _terminals = terminals;
            _sourceLines = new Dictionary<int, string>();
        }

        public void SetSourceLine(string sourceLine)
        {
            SetSourceLines(new List<string>() { sourceLine });
        }

        public void SetSourceLines(List<string> sourceLines)
        {
            for (int lineCounter = 1; lineCounter <= sourceLines.Count; lineCounter += 1)
            {
                _sourceLines.Add(lineCounter, sourceLines[lineCounter - 1]);
            }
        }

        [AllowNull]
        public Token CurrentToken
        {
            get
            {
                return _currentToken;
            }
            private set
            {
                _currentToken = value;
            }
        }

        public Token NextToken()
        {
            if (CurrentToken == null)
            {
                if (_sourceLines.Count != 0)
                {
                    _activeLineNumber = 1;
                }
                CurrentToken = new Token("BOF", "", _activeLineNumber, _activeCharacterNumber);
                return CurrentToken;
            }
            // Check if EOF (sourceline empty)
            else if (_sourceLines.Count == 0 && CurrentToken.Terminal.Equals("BOF"))
            {
                return new Token("EOF", "", _activeLineNumber, _activeCharacterNumber);
            }
            // Check if EOF (lastline EOL)
            else if (CurrentToken.Terminal.Equals("EOL") && _sourceLines.Count <= _activeLineNumber)
            {
                return new Token("EOF", "", _activeLineNumber, _activeCharacterNumber);
            }
            else if (CurrentToken.Terminal.Equals("BOF"))
            {
                CurrentToken = new Token("BOL", "", _activeLineNumber, _activeCharacterNumber);
                _activeCharacterNumber = 1;
                return CurrentToken;
            }
            // Check if BOL (lastline EOL)
            else if (CurrentToken.Terminal.Equals("EOL") && _sourceLines.Count > _activeLineNumber)
            {
                _activeLineNumber += 1;
                _activeCharacterNumber = 0;
                CurrentToken = new Token("BOL", "", _activeLineNumber, _activeCharacterNumber);
                _activeCharacterNumber += 1;
                return CurrentToken;
            }
            // check if EOL
            else if (_sourceLines[_activeLineNumber].Length < _activeCharacterNumber && !CurrentToken.Terminal.Equals("EOL"))
            {
                CurrentToken = new Token("EOL", "", _activeLineNumber, _activeCharacterNumber);
                return CurrentToken;
            }
            // check indent level change after EOL
            else if (indentCount(_sourceLines[_activeLineNumber]) != _activeIndentNumber)
            {
                if (indentCount(_sourceLines[_activeLineNumber]) > _activeIndentNumber)
                {
                    _activeIndentNumber += 1;
                    CurrentToken = new Token("INDENT_INCREASED", "", _activeLineNumber, _activeCharacterNumber);
                    return CurrentToken;
                }
                else
                {
                    _activeIndentNumber -= 1;
                    CurrentToken = new Token("INDENT_DECREASED", "", _activeLineNumber, _activeCharacterNumber);
                    return CurrentToken;
                }
            }
            else
            {
                // try matching all terminals
                TerminalMatch terminalMatch = null;

                // TODO Redo to make it more efficient than match all terminals
                foreach (Terminal terminal in _terminals)
                {
                    Match singleMatch = terminal.Match(_sourceLines[_activeLineNumber].Substring(_activeCharacterNumber - 1));
                    if (singleMatch.Success)
                    {
                        // TODO accept multible matches
                        terminalMatch = new TerminalMatch(terminal, singleMatch.Value);
                        break;
                    }
                }

                // if no match progress _activeCharacterNumber 1 and return unknownterminal
                if (terminalMatch == null)
                {
                    CurrentToken = new Token("UNKNOWN_TERMINAL", _sourceLines[_activeLineNumber].Substring(_activeCharacterNumber - 1, 1), _activeLineNumber, _activeCharacterNumber);
                    _activeCharacterNumber += 1;
                    return CurrentToken;
                }
                // if match is ok find terminal, progress _activeCharacterNumber and return token
                else
                {
                    if (!terminalMatch.IgnoreTerminal)
                    {
                        CurrentToken = new Token(terminalMatch.Terminal.TerminalName, terminalMatch.Capture, _activeLineNumber, _activeCharacterNumber);
                    }
                    _activeCharacterNumber += terminalMatch.Capture.Length;
                    if (terminalMatch.IgnoreTerminal)
                    {
                        return NextToken();
                    }
                    else
                    {
                        return CurrentToken;
                    }
                }
            }
        }

        private int indentCount(string activeLine)
        {
            // TODO count number of indents
            // TODO translate tab to spaces
            // TODO on empty lines return _activeIndentNumber
            return _activeIndentNumber;
        }

        /**
         * TODO Remove later
         */

        internal string TokenizeAll()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Token nextToken = NextToken();
            stringBuilder.AppendLine(nextToken.ToString());
            while (nextToken.Terminal != "EOF")
            {
                nextToken = NextToken();
                stringBuilder.AppendLine(nextToken.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
