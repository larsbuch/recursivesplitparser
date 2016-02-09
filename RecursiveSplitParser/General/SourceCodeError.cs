using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public class SourceCodeError
    {
        private string _errorText;
        private Exception _exception;

        public SourceCodeError(string errorText)
        {
            _errorText = errorText;
        }

        public SourceCodeError(string errorText, Exception exception)
        {
            _errorText = errorText;
            _exception = exception;
        }

        public override string ToString()
        {
            return _errorText;
        }
    }
}
