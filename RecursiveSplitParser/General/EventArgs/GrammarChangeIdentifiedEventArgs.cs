using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public class GrammarChangeIdentifiedEventArgs
    {
        public string GrammarName { get; set; }
        public int LexerPathID { get; set; }
    }
}
