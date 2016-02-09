using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public interface IParser
    {
        IObservable<CheckNextTokenEventArgs> CheckNextToken { get; }
        void OnCheckNextToken(CheckNextTokenEventArgs checkNextTokenEventArgs);
        IObservable<GrammarChangeIdentifiedEventArgs> GrammarChangeIdentified { get; }
        void OnGrammarChangeIdentified(GrammarChangeIdentifiedEventArgs grammarChangeIdentifiedEventArgs);
        void RegisterLexer(IScheduler scheduler, ILexer lexer);
    }
}
