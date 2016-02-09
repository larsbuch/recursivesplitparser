using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveSplitParser
{
    public abstract class AbstractLexerOptions
    {
        #region Send Event OptionsChanged

        private Subject<ILexerOptions> _optionsChanged = new Subject<ILexerOptions>();

        public IObservable<ILexerOptions> OptionsChanged
        {
            get
            {
                return _optionsChanged.AsObservable();
            }
        }

        public void NotifyOptionsChanged(ILexerOptions lexerOptions)
        {
            try
            {
                // If checks need to be made

                _optionsChanged.OnNext(lexerOptions);
            }
            catch (Exception exception)
            {
                _optionsChanged.OnError(exception);
            }
        }

        #endregion
    }
}
