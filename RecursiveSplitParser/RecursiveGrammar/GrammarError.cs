using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class GrammarError
    {
        private string _errorText;
        private Exception _exception;
        private string _grammarName;
        private int _lineNumber;

        public GrammarError(string grammarName, string errorText)
        {
            _grammarName = grammarName;
            _errorText = errorText;
        }

        public GrammarError(string grammarName, string errorText, Exception exception)
        {
            _grammarName = grammarName;
            _errorText = errorText;
            _exception = exception;
        }

        public GrammarError(string grammarName, int lineNumber, string errorText)
        {
            _grammarName = grammarName;
            _lineNumber = lineNumber;
            _errorText = errorText;
        }

        public override string ToString()
        {
            return _grammarName + getLineNumber() + ": " + _errorText;

        }

        private string getLineNumber()
        {
            if (_lineNumber > 0)
            {
                return " line: " + _lineNumber.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
