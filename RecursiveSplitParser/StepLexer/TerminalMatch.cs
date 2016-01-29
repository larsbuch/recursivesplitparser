using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class TerminalMatch
    {
        public bool IgnoreTerminal
        {
            get
            {
                return Terminal.IgnoreTerminal;
            }
        }
        public Terminal Terminal { get; private set; }
        public string Capture { get; private set; }

        public TerminalMatch(Terminal terminal, string capture)
        {
            Terminal = terminal;
            Capture = capture;
        }
    }
}
