using NullGuard;
using RecursiveGrammar;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
     * TODOs terminal replacement
     * 
     */
    public class StepLexer: IStepLexer
    {
        private ObservableCollection<Terminal> _terminals;
        private ReactiveCollection<string> _sourceLines;
        private StepLexerOptions _stepLexerOptions;
        private Dictionary<int, LexerPath> _lexerPathMap;
        protected static Regex _indentRegex;

        protected static Regex IndentRegex
        {
            get
            {
                if(_indentRegex == null)
                {
                    _indentRegex = new Regex("^([\t ]*)([^\t \n]+)$", RegexOptions.Compiled);
                }
                return _indentRegex;
            }
        }

        public StepLexer(RecursiveGrammarContainer recursiveGrammarContainer, ObservableCollection<string> sourceLines, StepLexerOptions stepLexerOptions)
        {
            _terminals = terminals;
            _terminals.CollectionChanged += ResetLexerFromCollection;
            _sourceLines = sourceLines;
            _sourceLines.CollectionChanged += ResetLexerFromCollection;
            _stepLexerOptions = stepLexerOptions;
            _stepLexerOptions.OptionsChanged += ResetLexer;
        }

        protected void ResetLexerFromCollection(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetLexer();
        }

        public void ResetLexer(object sender, EventArgs e)
        {
            ResetLexer();
        }

        public event EventHandler<TokensFoundEventArgs> TokensFound;

        protected virtual void OnTokenFound(TokensFoundEventArgs e)
        {
            EventHandler<TokensFoundEventArgs> handler = TokensFound;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        protected Dictionary<int, LexerPath> LexerPathMap
        {
            get
            {
                if(_lexerPathMap == null)
                {
                    _lexerPathMap = new Dictionary<int, LexerPath>();
                }
                return _lexerPathMap;
            }
        }

        protected int CurrentLexerPathId { get; set; }

        protected LexerPath ConfigureNewLexerPath(LexerPath lexerPath)
        {
            CurrentLexerPathId += 1;
            lexerPath.CurrentToken.LexerPathId = CurrentLexerPathId;
            lexerPath.LexerPathID = CurrentLexerPathId;
            LexerPathMap.Add(CurrentLexerPathId, lexerPath);
            return lexerPath;
        }

        protected void NewStartLexerPath()
        {
            ConfigureNewLexerPath(LexerPath.StartLexerPath);
        }

        protected void ResetLexer()
        {
            LexerPathMap.Clear();
            CurrentLexerPathId = LexerPath.NOTSET;
            NewStartLexerPath();
        }

        public void CheckNextToken(object sender, CheckNextTokenEventArgs e)
        {
            List<Token> nextTokens;
            if (e.LexerPathId == CheckNextTokenEventArgs.ALLPATHS)
            {
                nextTokens = NextTokens();
            }
            else
            {
                nextTokens = NextTokens(e.LexerPathId);
            }
            //TokensFoundEventArgs eventArgs = new TokensFoundEventArgs();
            //eventArgs.PreviousToken = CurrentToken;
            //Token[] tokens = NextTokens();
            //switch (tokens.Length)
            //{
            //    case 0:
            //        CurrentToken = new Token(Token.NULL, string.Empty, -1, 0);
            //        eventArgs.IdentifiedTokens = CurrentToken;
            //        break;
            //    case 1:
            //        CurrentToken = tokens[0];
            //        eventArgs.IdentifiedTokens = tokens;
            //        break;
            //    default:
            //        CurrentToken = tokens[0];
            //        eventArgs.IdentifiedTokens = tokens;
            //        break;
            //}
            //eventArgs.IdentifiedTokens = tokens;
            //OnTokenFound(eventArgs);
        }

        protected List<Token> NextTokens(int lexerPathId)
        {
            List<Token> tokenList = new List<Token>();
            LexerPath lexerPath;
            if(LexerPathMap.TryGetValue(lexerPathId, out lexerPath))
            {
                NextTokens(lexerPath, tokenList);
            }
            else
            {
                throw new StepLexerException(string.Format("LexerPath with id {0} does not exist.", lexerPathId));
            }
            return tokenList;
        }

        protected List<Token> NextTokens()
        {
            List<Token> tokenList = new List<Token>();
            foreach (LexerPath lexerPath in LexerPathMap.Values)
            {
                NextTokens(lexerPath, tokenList);
            }
            return tokenList;
        }

        protected void NextTokens(LexerPath lexerPath, List<Token> tokenList)
        {
            if (lexerPath.CurrentToken.Terminal.Equals(Token.NULL))
            {
                if (_sourceLines.Count != 0)
                {
                    lexerPath.ActiveLineNumber = Token.LINEPOSITION_START;
                }
                lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.BOF, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                tokenList.Add(lexerPath.CurrentToken);
            }
            // Check if EOF (sourceline empty)
            else if (_sourceLines.Count == 0 && lexerPath.CurrentToken.Terminal.Equals(Token.BOF))
            {
                lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.EOF, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                tokenList.Add(lexerPath.CurrentToken);
            }
            // Check if EOF (lastline EOL)
            else if (lexerPath.CurrentToken.Terminal.Equals(Token.EOL) && _sourceLines.Count <= lexerPath.ActiveLineNumber)
            {
                lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.EOF, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                tokenList.Add(lexerPath.CurrentToken);
            }
            else if (lexerPath.CurrentToken.Terminal.Equals(Token.BOF))
            {
                lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.BOL, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                lexerPath.ActiveCharacterNumber = Token.CHARPOSITION_START;
                tokenList.Add(lexerPath.CurrentToken);
            }
            // Check if BOL (lastline EOL)
            else if (lexerPath.CurrentToken.Terminal.Equals(Token.EOL) && _sourceLines.Count > lexerPath.ActiveLineNumber)
            {
                lexerPath.ActiveLineNumber += 1;
                lexerPath.ActiveCharacterNumber = Token.CHARPOSITION_START;
                lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.BOL, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                tokenList.Add(lexerPath.CurrentToken);
            }
            // check if EOL
            else if (_sourceLines[lexerPath.ActiveLineNumber].Length < lexerPath.ActiveCharacterNumber && !lexerPath.CurrentToken.Terminal.Equals(Token.EOL))
            {
                lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.EOL, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                tokenList.Add(lexerPath.CurrentToken);
            }
            // check indent level change after EOL
            else if (_stepLexerOptions.ReturnIndentToken)
            {
                int indentCount = CountIndent(lexerPath, _sourceLines[lexerPath.ActiveLineNumber]);
                if (indentCount != lexerPath.ActiveIndentNumber)
                {

                    if (indentCount > lexerPath.ActiveIndentNumber)
                    {
                        lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.INDENT_INCREASED, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                        tokenList.Add(lexerPath.CurrentToken);
                    }
                    else
                    {
                        lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.INDENT_DECREASED, string.Empty, lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber);
                        tokenList.Add(lexerPath.CurrentToken);
                    }
                    lexerPath.ActiveIndentNumber = indentCount;
                }
            }
            else
            {
                List<TerminalMatch> terminalMatches = new List<TerminalMatch>();

                // TODO Redo to make it more efficient than match all terminals
                foreach (Terminal terminal in _terminals)
                {
                    Match singleMatch = terminal.Match(_sourceLines[lexerPath.ActiveLineNumber].Substring(lexerPath.ActiveCharacterNumber));
                    if (singleMatch.Success)
                    {
                        terminalMatches.Add(new TerminalMatch(terminal, singleMatch.Value));
                    }
                }

                if (terminalMatches.Count == 0)
                {
                    if (LexerPathMap.Count > 1)
                    {
                        /* remove lexer path as it was earlier dublicated */
                        LexerPathMap.Remove(lexerPath.LexerPathID);
                    }
                    else
                    {
                        // if no match progress ActiveCharacterNumber 1 and return unknownterminal
                        lexerPath.CurrentToken = new Token(lexerPath.LexerPathID, Token.UNKNOWN_TERMINAL, _sourceLines[lexerPath.ActiveLineNumber].Substring(lexerPath.ActiveCharacterNumber, 1), lexerPath.ActiveLineNumber, lexerPath.ActiveCharacterNumber + 1);
                        lexerPath.ActiveCharacterNumber += 1;
                        tokenList.Add(lexerPath.CurrentToken);
                    }
                }
                else
                {
                    int matchCounter = 0;
                    // if match is ok find terminal, progress _activeCharacterNumber and add token
                    foreach (TerminalMatch terminalMatch in terminalMatches)
                    {
                        matchCounter += 1;
                        LexerPath newLexerPath = lexerPath;
                        if(terminalMatches.Count > 1 && matchCounter > 1)
                        {
                            newLexerPath = ConfigureNewLexerPath(newLexerPath.Clone());
                        }
                        if (!terminalMatch.IgnoreTerminal)
                        {
                            newLexerPath.CurrentToken = new Token(newLexerPath.LexerPathID, terminalMatch.Terminal.TerminalName, terminalMatch.Capture, newLexerPath.ActiveLineNumber, newLexerPath.ActiveCharacterNumber);
                        }
                        newLexerPath.ActiveCharacterNumber += terminalMatch.Capture.Length;
                        if (terminalMatch.IgnoreTerminal)
                        {
                            NextTokens(newLexerPath, tokenList);
                        }
                        else
                        {
                            tokenList.Add(newLexerPath.CurrentToken);
                        }
                    }
                }
            }
        }

        private int CountIndent(LexerPath lexerPath, string sourceLine)
        {

            Match match = IndentRegex.Match(sourceLine);
            if(match.Success)
            {
                Group group = match.Groups[1];
                string indentString = group.Value;
                int indentCount = indentString.Count(x => x == '\t') * _stepLexerOptions.IndentSpacePerTab;
                indentCount += indentString.Count(x => x == ' ');

                return indentCount;
            }
            else
            {
                return lexerPath.ActiveIndentNumber;
            }
        }
    }
}
