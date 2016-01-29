using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser
{
    public class CheckNextTokenEventArgs: ICheckNextTokenEventArgs
    {
        public int LexerPathId { get; internal set; }
    }
}
