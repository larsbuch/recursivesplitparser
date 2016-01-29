using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser
{
    public class SourceCodeLoader
    {
        private static string CODEDIRECTORY = "./Examples/";

        public SourceCodeList loadCodeFile(string fileName)
        {
            List<string> returnList = new List<string>();
            SourceCodeError codeError = null;
            if (File.Exists(CODEDIRECTORY + fileName))
            {
                try
                {
                    returnList = File.ReadAllLines(CODEDIRECTORY + fileName).ToList();
                }
                catch (Exception e)
                {
                    codeError = new SourceCodeError("Code file loading failed", e);
                }
            }
            else
            {
                codeError = new SourceCodeError("Code file does not exist in \"" + CODEDIRECTORY + "\"");
            }
            SourceCodeList codeList = loadSourceCode(returnList);
            if (codeError != null)
            {
                codeList.SourceCodeErrors.Add(codeError);
            }
            return codeList;
        }

        public SourceCodeList loadSourceCode(List<string> codeText)
        {
            SourceCodeList sourceCodeList = new SourceCodeList();
            foreach (string codeLine in codeText)
            {
                sourceCodeList.Add(new SourceCodeLine(codeLine));
            }
            return sourceCodeList;
        }
    }
}
