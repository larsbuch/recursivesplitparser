using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class GrammarLine
    {
        private int _lineCounter;
        private string _lineText;
        private string _grammarName;

        public GrammarLine(string grammarName, int lineCounter, string lineText)
        {
            _grammarName = grammarName;
            _lineCounter = lineCounter;
            _lineText = lineText;
        }

        public void writeLine(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(ToString());
        }

        public override string ToString()
        {
            return _grammarName + " line: " + _lineCounter + ": " + _lineText;
        }
    }
}
