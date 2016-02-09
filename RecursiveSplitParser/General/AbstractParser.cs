using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public abstract class AbstractParser:IParser, IDisposable
    {

        public void RegisterLexer(IScheduler scheduler, ILexer lexer)
        {
            CustomLexerEventSubscriptionRegister(scheduler , lexer);
            IgnoreTerminalSubscriptionRegister(scheduler, lexer);
            NextTokenSubscriptionRegister(scheduler, lexer);
        }

        public void Dispose()
        {
            CustomLexerEventSubscriptionDispose();
            IgnoreTerminalSubscriptionDispose();
            NextTokenSubscriptionDispose();
        }

        #region Events

        #region Send Event GrammarChangeIdentified

        private Subject<GrammarChangeIdentifiedEventArgs> _grammarChangeIdentified = new Subject<GrammarChangeIdentifiedEventArgs>();

        public IObservable<GrammarChangeIdentifiedEventArgs> GrammarChangeIdentified
        {
            get
            {
                return _grammarChangeIdentified.AsObservable();
            }
        }

        public void OnGrammarChangeIdentified(GrammarChangeIdentifiedEventArgs grammarChangeIdentifiedEventArgs)
        {
            try
            {
                // If checks need to be made

                _grammarChangeIdentified.OnNext(grammarChangeIdentifiedEventArgs);
            }
            catch (Exception exception)
            {
                _grammarChangeIdentified.OnError(exception);
            }
        }

        #endregion

        #region Send Event CheckNextToken

        private Subject<CheckNextTokenEventArgs> _checkNextToken = new Subject<CheckNextTokenEventArgs>();

        public IObservable<CheckNextTokenEventArgs> CheckNextToken
        {
            get
            {
                return _checkNextToken.AsObservable();
            }
        }

        public void OnCheckNextToken(CheckNextTokenEventArgs checkNextTokenEventArgs)
        {
            try
            {
                // If checks need to be made

                _checkNextToken.OnNext(checkNextTokenEventArgs);
            }
            catch (Exception exception)
            {
                _checkNextToken.OnError(exception);
            }
        }

        #endregion

        #region Receive Event CustomLexerEvent

        private IDisposable _customLexerEventSubstription;

        protected void CustomLexerEventSubscriptionRegister(IScheduler scheduler, ILexer lexer)
        {
            _customLexerEventSubstription = lexer.CustomLexerEvent.Subscribe(OnCustomLexerEvent, OnCustomLexerEventError);
        }

        /* remember to add to Dispose subscription */
        protected void CustomLexerEventSubscriptionDispose()
        {
            _customLexerEventSubstription.Dispose();
        }

        protected abstract void OnCustomLexerEvent(LexerCustomEventArgs lexerCustomEventArgs);
        protected abstract void OnCustomLexerEventError(Exception exception);

        #endregion

        #region Receive Event OnIgnoreTerminal

        private IDisposable _ignoreTerminalSubstription;

        protected void IgnoreTerminalSubscriptionRegister(IScheduler scheduler, ILexer lexer)
        {
            _ignoreTerminalSubstription = lexer.IgnoreTerminal.Subscribe(OnIgnoreTerminal, OnIgnoreTerminalError);
        }

        /* remember to add to Dispose subscription */
        protected void IgnoreTerminalSubscriptionDispose()
        {
            _ignoreTerminalSubstription.Dispose();
        }

        protected abstract void OnIgnoreTerminal(IgnoreTerminalEventArgs ignoreTerminalEventArgs);
        protected abstract void OnIgnoreTerminalError(Exception exception);

        #endregion

        #region Receive Event NextToken

        private IDisposable _nextTokenSubstription;

        protected void NextTokenSubscriptionRegister(IScheduler scheduler, ILexer lexer)
        {
            _nextTokenSubstription = lexer.NextToken.Subscribe(OnNextToken, OnNextTokenError, OnNextTokenCompeted);
        }

        /* remember to add to Dispose subscription */
        protected void NextTokenSubscriptionDispose()
        {
            _nextTokenSubstription.Dispose();
        }

        protected abstract void OnNextToken(List<IToken> tokenList);
        protected abstract void OnNextTokenError(Exception exception);
        protected abstract void OnNextTokenCompeted();

        #endregion

        #endregion
    }
}
