using System;

namespace StepLexer
{
    public class StepLexerOptions
    {
        private bool _returnIndentToken = false;
        private int _indentSpacePerTab = 4;

        public event EventHandler OptionsChanged;

        protected virtual void OnOptionsChanged(EventArgs e)
        {
            EventHandler handler = OptionsChanged;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        public bool ReturnIndentToken
        {
            get
            {
                return _returnIndentToken;
            }
            set
            {
                _returnIndentToken = value;
                OnOptionsChanged(EventArgs.Empty);
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
                OnOptionsChanged(EventArgs.Empty);
            }
        }
    }
}