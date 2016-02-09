using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface ILexer
    {
        IObservable<List<IToken>> NextToken { get; }

        void SendNextTokens(List<IToken> tokens);
        IObservable<IgnoreTerminalEventArgs> IgnoreTerminal { get; }

        void OnIgnoreTerminal(IgnoreTerminalEventArgs ignoreTerminal);

        IObservable<LexerCustomEventArgs> CustomLexerEvent { get; }

        void OnCustomLexerEvent(LexerCustomEventArgs splitLexerPath);
    }
}

