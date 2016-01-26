using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammar
{
    public class Grammar
    {
        private List<Production> _grammar;
        private List<Terminal> _terminals;

        public Grammar()
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
