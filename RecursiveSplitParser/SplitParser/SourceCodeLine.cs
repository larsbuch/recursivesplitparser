using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser
{
    public class SourceCodeLine
    {
        private string _line;

        public SourceCodeLine(string line)
        {
            _line = line;
        }

        public string writeCodeLine()
        {
            return _line;
        }
    }
}
