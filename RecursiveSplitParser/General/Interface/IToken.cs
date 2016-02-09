using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface IToken
    {
        string Match { get; }
        string Terminal { get; }
        int LinePosition { get; }
        int CharPosition { get; }
        int LexerPathId { get; }
    }
}
