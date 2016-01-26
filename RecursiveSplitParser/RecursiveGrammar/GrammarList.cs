using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammar
{
    public class GrammarList
    {
        private List<GrammarLine> _grammarLines;

        public GrammarList()
        {
            _grammarLines = new List<GrammarLine>();
        }

        public void writeGrammar(StringBuilder stringBuilder)
        {
            foreach (GrammarLine grammarLine in _grammarLines)
            {
                grammarLine.writeLine(stringBuilder);
            }
        }

        public void Add(GrammarLine grammarLine)
        {
            _grammarLines.Add(grammarLine);
        }
    }
}
