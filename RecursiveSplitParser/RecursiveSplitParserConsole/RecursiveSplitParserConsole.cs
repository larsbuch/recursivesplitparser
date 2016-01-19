using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParserConsole
{
    public class RecursiveSplitParserConsole
    {
        static void Main(string[] args)
        {
            if(args == null || args.Length != 3)
            {
                WriteLine(string.Format("Use: {0} {1} {2}", "SplitParserCmd", "GrammarFileAndPath", "ParseFileAndPath"));
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
