using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class GrammarContainer: IGrammarContainer
    {
        private static IGrammar _baseGrammar;
        public static IGrammar BaseGrammar
        {
            get
            {
                if(_baseGrammar == null)
                {
                    // TODO Make base grammar work
                    _baseGrammar = new RecursiveGrammar();
                }
                return _baseGrammar;
            }
        }

        public IGrammar GetGrammar(string grammarName)
        {
            throw new NotImplementedException();
        }

        public IGrammar GetBaseGrammar()
        {
            return BaseGrammar;
        }
    }
}
