using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grammar
{
    public class Terminal :ITerminal
    {
        public string TerminalName { get; private set; }
        public string TerminalMatch { get; private set; }
        public bool IgnoreTerminal { get; private set; }

        private Regex _regex;

        public Terminal(string terminalName, string terminalMatch, bool ignoreTerminal)
        {
            TerminalName = terminalName;
            TerminalMatch = terminalMatch;
            IgnoreTerminal = ignoreTerminal;
            _regex = new Regex("^" + terminalMatch, RegexOptions.Compiled);
        }


        public Match Match(string textToMatch)
        {
            return _regex.Match(textToMatch);
        }
    }
}
