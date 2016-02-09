using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface ILexerOptions
    {
        IObservable<ILexerOptions> OptionsChanged { get; }
        void NotifyOptionsChanged(ILexerOptions lexerOptions);
        bool ReturnIndentToken { get; }
        int IndentSpacePerTab { get; }
    }
}
