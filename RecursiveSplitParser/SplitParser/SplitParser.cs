using Grammar;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser
{
    public class SplitParser:IParser
    {
        public GrammarInterpreter Grammar { get; private set; }

        public void parseGrammarFile(string fileName)
        {
            GrammarLoader grammarLoader = new GrammarLoader();
            Grammar = grammarLoader.loadGrammarFile(fileName);
            Grammar.parseGrammar();
        }

        public bool HasGrammarErrors
        {
            get
            {
                return GrammarErrors.Count > 0;
            }
        }

        public List<GrammarError> GrammarErrors
        {
            get
            {
                return Grammar.GrammarErrors;
            }
        }

        public SourceCodeList SourceCode { get; private set; }

        public void parseCodeText(List<string> codeText)
        {
            SourceCodeLoader codeLoader = new SourceCodeLoader();
            SourceCode = codeLoader.loadSourceCode(codeText);
            parseCode();
        }

        public void parseCodeFile(string fileName)
        {
            SourceCodeLoader codeLoader = new SourceCodeLoader();
            SourceCode = codeLoader.loadCodeFile(fileName);
            parseCode();
        }

        private void parseCode()
        {
            if (SourceCode != null)
            {
                SourceCode.parseCode(Grammar);
            }
        }

        public bool HasCodeErrors
        {
            get
            {
                return SourceCodeErrors.Count > 0;
            }
        }

        public List<SourceCodeError> SourceCodeErrors
        {
            get
            {
                return SourceCode.SourceCodeErrors;
            }
        }

        public string writeCode()
        {
            return SourceCode.writeCode();
        }

        public string writeGrammar()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Grammar.writeGrammar(stringBuilder);
            return stringBuilder.ToString();
        }
    }
}
