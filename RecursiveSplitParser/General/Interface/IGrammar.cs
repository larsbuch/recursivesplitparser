using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface IGrammar
    {
        IEnumerable<ITerminal> Terminals { get; }
    }
}
