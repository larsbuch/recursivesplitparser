using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public class GuardClauses
    {
        public static void ListNotEmpty(IList list, string errorMessage)
        {
            if(list.Count < 1)
            {
                throw new GuardClausesException(errorMessage);
            }
        }

        public static void StringNotNullOrEmpty(string sourceLine, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(sourceLine))
            {
                throw new GuardClausesException(string.Format(errorMessage, sourceLine));
            }
        }
    }
}
