using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public abstract class AbstractLexer : ILexer, IDisposable
    {
        protected ReactiveList<SourceCodeLine> _sourceLines;
        protected IGrammarContainer _grammarContainer;
        protected IParser _parser;
        protected ILexerOptions _lexerOptions;

        public AbstractLexer(IScheduler scheduler ,IParser parser, IGrammarContainer grammarContainer, ReactiveList<SourceCodeLine> sourceLines, ILexerOptions lexerOptions)
        {
            _parser = parser;
            CheckNextTokenSubscriptionRegister(scheduler, parser);
            GrammarChangeIdentifiedSubscriptionRegister(scheduler, parser);
            _grammarContainer = grammarContainer;
            _sourceLines = sourceLines;
            SourceLinesChangedSubscriptionRegister(scheduler, sourceLines);
            _lexerOptions = lexerOptions;
            OptionsChangedSubscriptionRegister(scheduler, lexerOptions);
            _parser.RegisterLexer(scheduler, this);
        }

        public void Dispose()
        {
            OptionsChangedSubscriptionDispose();
            CheckNextTokenSubscriptionDispose();
            SourceLinesChangedSubscriptionDispose();
            GrammarChangeIdentifiedSubscriptionDispose();
        }


        #region Events

        #region Recieve Event SourceLinesChanged

        private IDisposable _sourceLinesChangedSubscription;

        protected void SourceLinesChangedSubscriptionRegister(IScheduler scheduler, ReactiveList<SourceCodeLine> sourceLines)
        {
            _sourceLinesChangedSubscription = sourceLines.Changed.Subscribe(OnSourceLinesChanged, OnSourceLinesChangedError);
        }

        /* remember to add to Dispose subscription */
        protected void SourceLinesChangedSubscriptionDispose()
        {
            _sourceLinesChangedSubscription.Dispose();
        }


        protected abstract void OnSourceLinesChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs);
        protected abstract void OnSourceLinesChangedError(Exception exception);

        #endregion

        #region Receive Event CheckNextToken

        private IDisposable _checkNextTokenSubstription;

        protected void CheckNextTokenSubscriptionRegister(IScheduler scheduler, IParser parser)
        {
            _checkNextTokenSubstription = parser.CheckNextToken.Subscribe(OnCheckNextToken, OnCheckNextTokenError);
        }

        /* remember to add to Dispose subscription */
        protected void CheckNextTokenSubscriptionDispose()
        {
            _checkNextTokenSubstription.Dispose();
        }

        protected abstract void OnCheckNextToken(CheckNextTokenEventArgs args);
        protected abstract void OnCheckNextTokenError(Exception exception);

        #endregion

        #region Receive Event GrammarChangeIdentified

        private IDisposable _grammarChangeIdentifiedSubscription;

        protected void GrammarChangeIdentifiedSubscriptionRegister(IScheduler scheduler, IParser parser)
        {
            _grammarChangeIdentifiedSubscription = parser.GrammarChangeIdentified.Subscribe(OnGrammarChangeIdentified, OnGrammarChangeIdentifiedError);
        }

        /* remember to add to Dispose subscription */
        protected void GrammarChangeIdentifiedSubscriptionDispose()
        {
            _grammarChangeIdentifiedSubscription.Dispose();
        }

        protected abstract void OnGrammarChangeIdentified(GrammarChangeIdentifiedEventArgs args);
        protected abstract void OnGrammarChangeIdentifiedError(Exception exception);

        #endregion

        #region Receive Event OptionsChanged

        private IDisposable _optionsChangedSubstription;

        protected void OptionsChangedSubscriptionRegister(IScheduler scheduler, ILexerOptions lexerOptions)
        {
            _optionsChangedSubstription = lexerOptions.OptionsChanged.Subscribe(OnOptionsChanged, OnOptionsChangedError);
        }

        /* remember to add to Dispose subscription */
        protected void OptionsChangedSubscriptionDispose()
        {
            _optionsChangedSubstription.Dispose();
        }

        protected abstract void OnOptionsChanged(ILexerOptions lexerOptions);
        protected abstract void OnOptionsChangedError(Exception exception);

        #endregion

        #region Send Event NextToken

        private Subject<List<IToken>> _nextToken = new Subject<List<IToken>>();

        public IObservable<List<IToken>> NextToken
        {
            get
            {
                return _nextToken.AsObservable();
            }
        }

        protected void AllTokensFound()
        {
            _nextToken.OnCompleted();
        }

        public void SendNextTokens(List<IToken> tokens)
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

        #region Send Event OnIgnoreTerminal

        private Subject<IgnoreTerminalEventArgs> _ignoreTerminal = new Subject<IgnoreTerminalEventArgs>();

        public IObservable<IgnoreTerminalEventArgs> IgnoreTerminal
        {
            get
            {
                return _ignoreTerminal.AsObservable();
            }
        }

        public void OnIgnoreTerminal(IgnoreTerminalEventArgs ignoreTerminal)
        {
            try
            {
                // If checks need to be made

                _ignoreTerminal.OnNext(ignoreTerminal);
            }
            catch (Exception exception)
            {
                _ignoreTerminal.OnError(exception);
            }
        }

        #endregion

        #region Send Event OnCustomLexerEvent

        private Subject<LexerCustomEventArgs> _customLexerEvent = new Subject<LexerCustomEventArgs>();

        public IObservable<LexerCustomEventArgs> CustomLexerEvent
        {
            get
            {
                return _customLexerEvent.AsObservable();
            }
        }

        public void OnCustomLexerEvent(LexerCustomEventArgs splitLexerPath)
        {
            try
            {
                // If checks need to be made

                _customLexerEvent.OnNext(splitLexerPath);
            }
            catch (Exception exception)
            {
                _customLexerEvent.OnError(exception);
            }
        }

        #endregion

        #endregion
    }
}
