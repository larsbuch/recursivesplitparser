using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class GrammarLoader
    {
        private static string GRAMMARSTART = "Grammar:";
        //        private static string INCLUDESTART = "Include:";
        private static string TOKENSPLITTERSTART = "TokenSplitter:";
        private static string BASEGRAMMARNAME = "BNF";
        private static string GRAMMARDIRECTORY = "./Grammar/";
        private static string GRAMMARENDING = ".grammar";
        //        private static string EXTENSIONDIRECTORY = "./Extensions/";
        //        private static string EXTENSIONENDING = ".extension";

        public GrammarInterpreter loadGrammarFile(string grammarName)
        {
            List<GrammarError> grammarErrors = new List<GrammarError>();

            return loadGrammar(grammarName, loadGrammarFile(GRAMMARDIRECTORY, grammarName, GRAMMARENDING, grammarErrors), grammarErrors);
        }

        public GrammarInterpreter loadGrammar(string fileName, List<string> lineList)
        {
            List<GrammarError> grammarErrors = new List<GrammarError>();

            return loadGrammar(fileName, lineList, grammarErrors);
        }

        private GrammarInterpreter loadGrammar(string grammarName, List<string> lineList, List<GrammarError> grammarErrors)
        {
            GrammarInterpreter grammarInterpreter = new GrammarInterpreter(grammarErrors);
            return loadGrammar(grammarInterpreter, grammarName, lineList);
        }

        private GrammarInterpreter loadGrammar(GrammarInterpreter grammarInterpreter, string grammarName, List<string> lineList)
        {
            if (lineList == null)
            {
                throw new ArgumentNullException("lineList must exist", "lineList");
            }

            grammarInterpreter.GrammarName = grammarName;
            int lineNumber = 1;
            // read first line
            string firstLine = "";
            if (lineList.Count >= lineNumber)
            {
                firstLine = lineList[lineNumber - 1];
                if (IsGrammarDefinitionLine(firstLine))
                {
                    // Create parent grammar interpreter
                    parseGrammarDefinitionLine(grammarInterpreter, firstLine);
                }
                else
                {
                    grammarInterpreter.addError(new GrammarError(grammarName, lineNumber, "Expected grammar definition line"));
                }
                lineNumber += 1;
            }

            // read second line 
            string secondLine = "";
            if (lineList.Count >= lineNumber)
            {
                secondLine = lineList[lineNumber - 1];
                if (IsTokenSplitterDefinitionLine(secondLine))
                {
                    lineNumber += 1;
                    // Add extensions if needed
                    parseTokenSplitterLine(grammarInterpreter, secondLine);
                }
                else if (IsEmptyLine(secondLine))
                {
                    // Do nothing
                }
                else
                {
                    grammarInterpreter.addError(new GrammarError(grammarName, lineNumber, "Expected empty line or token splitter line"));
                }
            }
            // read second or third line
            string secondOrThirdLine = "";
            if (lineList.Count >= lineNumber)
            {
                secondOrThirdLine = lineList[lineNumber - 1];
                if (IsEmptyLine(secondOrThirdLine))
                {
                    // do nothing
                }
                else
                {
                    grammarInterpreter.addError(new GrammarError(grammarName, lineNumber, "Expected empty line"));
                }
                lineNumber += 1;
            }
            grammarInterpreter.addGrammar(grammarName, lineNumber, lineList);
            return grammarInterpreter;
        }

        private bool IsEmptyLine(string line)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
        private void parseExtensionLine(GrammarInterpreter grammarInterpreter, string secondLine)
        {
            string extensionLine = secondLine.TrimStart(INCLUDESTART.ToCharArray()).Trim();
            string[] extensionNames = extensionLine.Split(',');
            string grammarExtensionName = "";
            foreach (string extensionName in extensionNames)
            { 
                grammarExtensionName = extensionName.Trim();
                // load grammar extension
                List<string> extensionLines = loadGrammarFile(EXTENSIONDIRECTORY, grammarExtensionName, EXTENSIONENDING, grammarInterpreter.GrammarErrors);
                loadGrammar(grammarInterpreter, grammarExtensionName, extensionLines);
            }
        }


        private bool IsExtensionDefinitionLine(string secondLine)
        {
            if (secondLine.StartsWith(INCLUDESTART))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */



        private void parseTokenSplitterLine(GrammarInterpreter grammarInterpreter, string textLine)
        {
            string tokenSplitterLine = textLine.TrimStart(TOKENSPLITTERSTART.ToCharArray()).Trim();
            switch (tokenSplitterLine)
            {
                case "None":
                    grammarInterpreter.TokenSplitter = TokenSplitterType.None;
                    break;
                case "Space":
                    grammarInterpreter.TokenSplitter = TokenSplitterType.Space;
                    break;
                default:
                    grammarInterpreter.TokenSplitter = TokenSplitterType.Regex;
                    grammarInterpreter.RegexTokenSplitter = tokenSplitterLine;
                    break;
            }
        }


        private bool IsTokenSplitterDefinitionLine(string textLine)
        {
            if (textLine.StartsWith(TOKENSPLITTERSTART))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void parseGrammarDefinitionLine(GrammarInterpreter grammarInterpreter, string textLine)
        {
            string grammarName = textLine.TrimStart(GRAMMARSTART.ToCharArray()).Trim();
            if (!grammarName.Equals(BASEGRAMMARNAME))
            {
                grammarInterpreter.addParentInterpreter(loadGrammarFile(grammarName));
            }
        }

        private bool IsGrammarDefinitionLine(string textLine)
        {
            if (textLine.StartsWith(GRAMMARSTART))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<string> loadGrammarFile(string grammarDirectory, string grammarName, string grammarEnding, List<GrammarError> grammarErrors)
        {
            List<string> lineList = new List<string>();
            if (File.Exists(grammarDirectory + grammarName + grammarEnding))
            {
                try
                {
                    lineList = File.ReadAllLines(grammarDirectory + grammarName + grammarEnding).ToList();
                }
                catch (Exception e)
                {
                    grammarErrors.Add(new GrammarError(grammarName, "Grammar file loading failed", e));
                }
            }
            else
            {
                grammarErrors.Add(new GrammarError(grammarName, "Grammar file does not exist grammar directory"));
            }
            return lineList;
        }
    }
}
