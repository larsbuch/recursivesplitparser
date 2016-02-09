using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface ISourceCodeLine
    {
        int Length { get; }
        string Substring(int startIndex);
        string Substring(int startIndex, int length);
    }
}
