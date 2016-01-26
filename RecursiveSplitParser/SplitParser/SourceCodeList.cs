using RecursiveGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser
{
    public class SourceCodeList
    {
        private List<SourceCodeLine> _sourceCodeLines;

        public List<SourceCodeError> SourceCodeErrors { get; set; }

        public SourceCodeList()
        {
            _sourceCodeLines = new List<SourceCodeLine>();
            SourceCodeErrors = new List<SourceCodeError>();
        }

        public string writeCode()
        {
            StringBuilder returnString = new StringBuilder();
            foreach (SourceCodeLine codeLine in _sourceCodeLines)
            {
                returnString.AppendLine(codeLine.writeCodeLine());
            }

            return returnString.ToString();
        }

        public void parseCode(GrammarInterpreter Grammar)
        {
            // TODO parse code
        }

        public void Add(SourceCodeLine codeLine)
        {
            _sourceCodeLines.Add(codeLine);
        }
    }
}
