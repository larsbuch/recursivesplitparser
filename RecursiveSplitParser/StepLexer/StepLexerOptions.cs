using RecursiveSplitParser;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Lexer
{
    public class StepLexerOptions:AbstractLexerOptions,ILexerOptions
    {
        private bool _returnIndentToken = false;
        private int _indentSpacePerTab = 4;


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