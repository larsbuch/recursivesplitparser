using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecursiveSplitParser
{
    public class SourceCodeLoader
    {
        public void loadCodeFile(ReactiveList<SourceCodeLine> sourceCodeLines, string filePath, string fileName)
        {
            List<string> returnList = new List<string>();
            SourceCodeError codeError = null;
            if (File.Exists(filePath + GeneralResources.DIRECTORYSPLITTER + fileName))
            {
                try
                {
                    returnList = File.ReadAllLines(filePath + GeneralResources.DIRECTORYSPLITTER + fileName).ToList();
                }
                catch (Exception e)
                {
                    codeError = new SourceCodeError("Code file loading failed", e);
                }
            }
            else
            {
                codeError = new SourceCodeError("Code file does not exist in \"" + filePath + "\"");
            }
            loadSourceCode(sourceCodeLines, returnList);
        }

        public void loadSourceCode(ReactiveList<SourceCodeLine> sourceCodeLines, List<string> codeText)
        {
            foreach (string codeLine in codeText)
            {
                sourceCodeLines.Add(new SourceCodeLine(codeLine));
            }
        }
    }
}
