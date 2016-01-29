using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Lexer
{
    public class StepLexerOptions
    {
        private bool _returnIndentToken = false;
        private int _indentSpacePerTab = 4;

        #region Send Event OptionsChanged

        private Subject<StepLexerOptions> _optionsChanged = new Subject<StepLexerOptions>();

        public IObservable<StepLexerOptions> OptionsChanged
        {
            get
            {
                return _optionsChanged.AsObservable();
            }
        }

        public void NotifyOptionsChanged(StepLexerOptions stepLexerOptions)
        {
            try
            {
                // If checks need to be made

                _optionsChanged.OnNext(stepLexerOptions);
            }
            catch(Exception exception)
            {
                _optionsChanged.OnError(exception);
            }
        }

        #endregion

        public bool ReturnIndentToken
        {
            get
            {
                return _returnIndentToken;
            }
            set
            {
                _returnIndentToken = value;
                NotifyOptionsChanged(this);
            }
        }

        public int IndentSpacePerTab
        {
            get
            {
                return _indentSpacePerTab;
            }
            set
            {
                _indentSpacePerTab = value;
                NotifyOptionsChanged(this);
            }
        }
    }
}