using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class GrammarInterpreter
    {
        public List<GrammarError> GrammarErrors { get; set; }
        private GrammarInterpreter _parent;
        private GrammarList _grammarList;

        public void parseGrammar()
        {
            if (_parent != null)
            {
                _parent.parseGrammar();
            }
            // TODO parse grammar lines
        }

        public void addGrammar(string grammarName, int lineNumber, List<string> lineList)
        {
            for (int lineCounter = lineNumber; lineList.Count > lineCounter; lineCounter += 1)
            {
                _grammarList.Add(new GrammarLine(grammarName, lineCounter, lineList[lineCounter]));
            }
        }

        public void addError(GrammarError grammarError)
        {
            GrammarErrors.Add(grammarError);
        }

        public GrammarInterpreter(List<GrammarError> grammarErrors)
        {
            GrammarErrors = grammarErrors;
            _grammarList = new GrammarList();
        }

        public void addParentInterpreter(GrammarInterpreter grammarInterpreter)
        {
            _parent = grammarInterpreter;
        }

        public void writeGrammar(StringBuilder stringBuilder)
        {
            if (_parent != null)
            {
                _parent.writeGrammar(stringBuilder);
            }
            _grammarList.writeGrammar(stringBuilder);
        }

        public string GrammarName { get; set; }

        public TokenSplitterType TokenSplitter { get; set; }

        public string RegexTokenSplitter { get; set; }

        internal RecursiveGrammar GetGrammar()
        {
            throw new NotImplementedException();
        }
    }
}
