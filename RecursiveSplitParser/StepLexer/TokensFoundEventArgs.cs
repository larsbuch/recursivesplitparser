using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepLexer
{
    public class TokensFoundEventArgs : EventArgs
    {
        public Token[] IdentifiedTokens { get; set; }
    }
}
