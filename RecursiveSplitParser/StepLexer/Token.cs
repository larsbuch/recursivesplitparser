using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepLexer
{
    public class Token
    {
        public string Match { get; private set; }
        public string Terminal { get; private set; }
        public int LinePosition { get; private set; }
        public int CharPosition { get; private set; }

        public Token(string terminal, string match, int linePosition, int charPosition)
        {
            Match = match;
            Terminal = terminal;
            LinePosition = linePosition;
            CharPosition = charPosition;
        }

        public override string ToString()
        {
            return Terminal + ": " + Match + "(" + LinePosition + "," + CharPosition + ")";
        }
    }
}
