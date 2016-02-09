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
using ReactiveUI;

namespace Lexer
{
    /**
     * Responsible for tokenizing the input. Input is tokenized regex match done by NextToken() function. Using a stepwize 
     * lexer makes it possible to redefine terminals on the run making parsing Tex possible.
     * 
     */
    public class StepLexer: AbstractLexer
    {
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

        public StepLexer(IParser parser, IGrammarContainer grammarContainer, ReactiveList<SourceCodeLine> sourceLines, ILexerOptions lexerOptions):base(parser, grammarContainer, sourceLines, lexerOptions)
        {
        }


        #region Properties

        #endregion

        #region Events

        protected override void OnOptionsChanged(ILexerOptions lexerOptions)
        {
            Reset();
        }
        private void NotifySplitLexerPath(LexerPath newLexerPath)
        {
            LexerCustomEventArgs lexerCustomEventArgs = new LexerCustomEventArgs();
            lexerCustomEventArgs.EventName = StepLexerResources.LexerCustomEventArgs_SplitLexerPath;
            lexerCustomEventArgs.EventValue = newLexerPath.LexerPathID.ToString();
            OnCustomLexerEvent(lexerCustomEventArgs);
        }

        private void NotifyCollapsingLexerPath(LexerPath newLexerPath)
        {
            LexerCustomEventArgs lexerCustomEventArgs = new LexerCustomEventArgs();
            lexerCustomEventArgs.EventName = StepLexerResources.LexerCustomEventArgs_CollapseLexerPath;
            lexerCustomEventArgs.EventValue = newLexerPath.LexerPathID.ToString();
            OnCustomLexerEvent(lexerCustomEventArgs);
        }



        private void NotifyIgnoreTerminal(IToken token)
        {
            IgnoreTerminalEventArgs ignoreTerminal = new IgnoreTerminalEventArgs();
            ignoreTerminal.Token = token;
            OnIgnoreTerminal(ignoreTerminal);
        }

        protected override void OnGrammarChangeIdentified(GrammarChangeIdentifiedEventArgs args)
        {
            /* Change grammar in lexerpath */
            try
            {
                LexerPath lexerPath;
                if (_lexerPathMap.TryGetValue(args.LexerPathID, out lexerPath))
                {
                    lexerPath.ActiveGrammar = _grammarContainer.GetGrammar(args.GrammarName);
                }
            }
            catch (Exception e)
            {
                /* TODO Do something intelligent with exception */
            }
        }

        protected override void OnCheckNextToken(CheckNextTokenEventArgs args)
        {
            /* TODO Buffer changes to options, lexerPath */
            List<IToken> nextTokens;
            if (args.LexerPathId == StepLexerResources.LexerPathId_ALLPATHS)
            {
                nextTokens = NextTokens();
            }
            else
            {
                nextTokens = NextTokens(args.LexerPathId);
            }
            if (nextTokens.Count == 0)
            {
                AllTokensFound();
            }
            else
            {
                SendNextTokens(nextTokens);
            }
            /* TODO Allow buffers to send messages out */
        }

        protected override void OnSourceLinesChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            /* Always reset parsing when source lines change */
            Reset();
        }


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
            ConfigureNewLexerPath(LexerPath.StartLexerPath(_grammarContainer.GetBaseGrammar()));
        }

        protected void Reset()
        {
            LexerPathMap.Clear();
            CurrentLexerPathId = LexerPath.NOTSET;
            NewStartLexerPath();
        }

        protected List<IToken> NextTokens(int lexerPathId)
        {
            List<IToken> tokenList = new List<IToken>();
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

        protected List<IToken> NextTokens()
        {
            List<IToken> tokenList = new List<IToken>();
            foreach (LexerPath lexerPath in LexerPathMap.Values)
            {
                NextTokens(lexerPath, tokenList);
            }
            return tokenList;
        }

        protected void NextTokens(LexerPath lexerPath, List<IToken> tokenList)
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
            else if (_lexerOptions.ReturnIndentToken)
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
                        NotifyCollapsingLexerPath(lexerPath);
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
                            NotifySplitLexerPath(newLexerPath);
                        }
                        if (!terminalMatch.IgnoreTerminal)
                        {
                            newLexerPath.CurrentToken = new Token(newLexerPath.LexerPathID, terminalMatch.Terminal.TerminalName, terminalMatch.Capture, newLexerPath.ActiveLineNumber, newLexerPath.ActiveCharacterNumber);
                        }
                        else
                        {
                            NotifyIgnoreTerminal(new Token(newLexerPath.LexerPathID, terminalMatch.Terminal.TerminalName, terminalMatch.Capture, newLexerPath.ActiveLineNumber, newLexerPath.ActiveCharacterNumber));
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

        private int CountIndent(LexerPath lexerPath, ISourceCodeLine sourceCodeLine)
        {

            Match match = IndentRegex.Match(sourceCodeLine.ToString());
            if(match.Success)
            {
                Group group = match.Groups[1];
                string indentString = group.Value;
                int indentCount = indentString.Count(x => x == '\t') * _lexerOptions.IndentSpacePerTab;
                indentCount += indentString.Count(x => x == ' ');

                return indentCount;
            }
            else
            {
                return lexerPath.ActiveIndentNumber;
            }
        }

        protected override void OnSourceLinesChangedError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckNextTokenError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnGrammarChangeIdentifiedError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnOptionsChangedError(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
