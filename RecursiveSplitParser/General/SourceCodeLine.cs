using RecursiveSplitParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public class SourceCodeLine: ISourceCodeLine
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

        public int Length
        {
            get
            {
                return _line.Length;
            }
        }

        public string Substring(int startIndex)
        {
            return _line.Substring(startIndex);
        }

        public string Substring(int startIndex, int length)
        {
            return _line.Substring(startIndex, length);
        }

        public override string ToString()
        {
            return _line;
        }
    }
}
