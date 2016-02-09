using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface ITerminal
    {
        string TerminalName { get; }
        string TerminalMatch { get; }
        bool IgnoreTerminal { get; }
        Match Match(string textToMatch);
    }
}
