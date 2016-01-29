using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class RecursiveGrammar: IGrammar
    {
        private List<Production> _grammar;
        private List<Terminal> _terminals;

        public RecursiveGrammar()
        {
            _grammar = new List<Production>();
            _terminals = new List<Terminal>();
        }

        public List<Terminal> GetTerminals()
        {
            return _terminals;
        }
    }
}
