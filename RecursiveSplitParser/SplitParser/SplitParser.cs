using Grammar;
using ReactiveUI;
using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser
{
    public class SplitParser:AbstractParser
    {
        private ReactiveList<SourceCodeLine> _sourceCodeLines;
        public SplitParser()
        {
            _sourceCodeLines = new ReactiveList<SourceCodeLine>();
        }


        #region Properties

        public GrammarInterpreter Grammar { get; private set; }

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

        public ReactiveList<SourceCodeLine> SourceCodeLines
        {
            get
            {
                return _sourceCodeLines;
            }
        }

        #endregion


        public void parseGrammarFile(string fileName)
        {
            GrammarLoader grammarLoader = new GrammarLoader();
            Grammar = grammarLoader.loadGrammarFile(fileName);
            Grammar.parseGrammar();
        }


        public void parseCodeText(List<string> codeText)
        {
            SourceCodeLoader codeLoader = new SourceCodeLoader();
            codeLoader.loadSourceCode(SourceCodeLines, codeText);
            parseCode();
        }

        public void parseCodeFile(string filePath, string fileName)
        {
            SourceCodeLoader codeLoader = new SourceCodeLoader();
            codeLoader.loadCodeFile(SourceCodeLines, filePath, fileName);
            parseCode();
        }

        private void parseCode()
        {
            throw new NotImplementedException();
            //if (SourceCodeLines != null)
            //{
            //    SourceCodeLines.parseCode(Grammar);
            //}
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
                throw new NotImplementedException();
                //return SourceCodeLines.SourceCodeErrors;
            }
        }

        public string writeCode()
        {
            throw new NotImplementedException();
            //return SourceCodeLines.writeCode();
        }

        public string writeGrammar()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Grammar.writeGrammar(stringBuilder);
            return stringBuilder.ToString();
        }

        protected override void OnCustomLexerEvent(LexerCustomEventArgs lexerCustomEventArgs)
        {
            throw new NotImplementedException();
        }

        protected override void OnIgnoreTerminal(IgnoreTerminalEventArgs ignoreTerminalEventArgs)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextToken(List<IToken> tokenList)
        {
            throw new NotImplementedException();
        }

        protected override void OnCustomLexerEventError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnIgnoreTerminalError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextTokenError(Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextTokenCompeted()
        {
            throw new NotImplementedException();
        }
    }
}
