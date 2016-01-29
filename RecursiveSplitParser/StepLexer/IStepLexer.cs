using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepLexer
{
    public interface IStepLexer
    {
        void ResetLexer(object sender, EventArgs e);
        void CheckNextToken(object sender, CheckNextTokenEventArgs e);
    }
}
