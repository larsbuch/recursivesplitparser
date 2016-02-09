using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.UnitTests
{
    public class TestParser : AbstractParser
    {
        List<IToken> _tokens;
        List<IToken> _ignoredTokens;
        bool _lexerHasSplit = false;
        int _lexerConcurrentPath = 1;
        bool _lexerCompleted = false;

        public TestParser()
        {
            _tokens = new List<IToken>();
            _ignoredTokens = new List<IToken>();
        }
        protected override void OnCustomLexerEvent(LexerCustomEventArgs lexerCustomEventArgs)
        {
            if(lexerCustomEventArgs.EventName.Equals("SplitLexerPath"))
            {
                _lexerHasSplit = true;
                _lexerConcurrentPath += 1;
            }
            else if (lexerCustomEventArgs.EventName.Equals("CollapseLexerPath"))
            {
                _lexerConcurrentPath -= 1;
            }

        }

        protected override void OnIgnoreTerminal(IgnoreTerminalEventArgs ignoreTerminalEventArgs)
        {
            IgnoredTokens.Add(ignoreTerminalEventArgs.Token);
        }

        protected override void OnNextToken(List<IToken> tokenList)
        {
            foreach(IToken token in tokenList)
            {
                _tokens.Add(token);
            }
        }

        protected override void OnCustomLexerEventError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnIgnoreTerminalError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextTokenError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextTokenCompeted()
        {
            _lexerCompleted = true;
        }

        public List<IToken> Tokens
        {
            get
            {
                return _tokens;
            }
        }

        public List<IToken> IgnoredTokens
        {
            get
            {
                return _ignoredTokens;
            }
        }
        public bool LexerHasSplit
        {
            get
            {
                return _lexerHasSplit;
            }
        }
        public int LexerConcurrentPaths
        {
            get
            {
                return _lexerConcurrentPath;
            }
        }

        public bool LexerCompleted
        {
            get
            {
                return _lexerCompleted;
            }
        }

        public void Parse()
        {
            if(!LexerCompleted)
            {
                CheckNextTokenEventArgs checkNextTokenEventArgs = new CheckNextTokenEventArgs();
                checkNextTokenEventArgs.LexerPathId = 0;
                OnCheckNextToken(checkNextTokenEventArgs);
            }
        }

    }
}
