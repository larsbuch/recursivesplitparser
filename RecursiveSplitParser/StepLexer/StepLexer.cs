using NullGuard;
using Grammar;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Lexer
{
    /**
     * Responsible for tokenizing the input. Input is tokenized regex match done by NextToken() function. Using a stepwize 
     * lexer makes it possible to redefine terminals on the run making parsing Tex possible.
     * 
     */
    public class StepLexer: ILexer, IDisposable
    {
        private ObservableCollection<string> _sourceLines;
        private IGrammarContainer _grammarContainer;
        private IParser _parser;
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

        public StepLexer(IParser parser, IGrammarContainer grammarContainer, ObservableCollection<string> sourceLines, StepLexerOptions stepLexerOptions)
        {
            _parser = parser;
            _grammarContainer = grammarContainer;
            _sourceLines = sourceLines;
            _sourceLines.CollectionChanged += ResetLexerFromCollection;
            _stepLexerOptions = stepLexerOptions;
            OptionsChangedSubscriptionRegister();
        }

        public void Dispose()
        {
            OptionsChangedSubscriptionDispose();
            CheckNextTokenSubscriptionDispose();
        }

    #region Events

    #region Recieve Event CheckNextToken

    private IDisposable _checkNextTokenSubstription;

        private void CheckNextTokenSubscriptionRegister()
        {
            _checkNextTokenSubstription = _parser.CheckNextToken.Subscribe(OnCheckNextToken);
        }

        /* remember to add to Dispose subscription */
        private void CheckNextTokenSubscriptionDispose()
        {
            _checkNextTokenSubstription.Dispose();
        }

        private void OnCheckNextToken(ICheckNextTokenEventArgs args)
        {
            List<Token> nextTokens;
            if (args.LexerPathId == Constants.LexerPath_ALLPATHS)
            {
                nextTokens = NextTokens();
            }
            else
            {
                nextTokens = NextTokens(e.LexerPathId);
            }

            SendNextTokens(nextTokens);
        }

        #endregion

        #region Recieve Event OptionsChanged

        private IDisposable _optionsChangedSubstription;

        private void OptionsChangedSubscriptionRegister()
        {
            _optionsChangedSubstription = _stepLexerOptions.OptionsChanged.Subscribe(OnOptionsChanged);
        }

        /* remember to add to Dispose subscription */
        private void OptionsChangedSubscriptionDispose()
        {
            _optionsChangedSubstription.Dispose();
        }

        private void OnOptionsChanged(StepLexerOptions stepLexerOptions)
        {
            Reset();
        }

        #endregion

        #region Send Event NextToken

        private Subject<List<Token>> _nextToken = new Subject<List<Token>>();

        public IObservable<List<Token>> NextToken
        {
            get
            {
                return _nextToken.AsObservable();
            }
        }

        public void AllTokensFound()
        {
            _nextToken.OnCompleted();
        }

        public void SendNextTokens(List<Token> tokens)
        {
            try
            {
                // If checks need to be made

                _nextToken.OnNext(tokens);
            }
            catch (Exception exception)
            {
                _nextToken.OnError(exception);
            }
        }

        #endregion

        #endregion

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

        protected void Reset()
        {
            LexerPathMap.Clear();
            CurrentLexerPathId = LexerPath.NOTSET;
            NewStartLexerPath();
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
                foreach (Terminal terminal in lexerPath.Terminals)
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
