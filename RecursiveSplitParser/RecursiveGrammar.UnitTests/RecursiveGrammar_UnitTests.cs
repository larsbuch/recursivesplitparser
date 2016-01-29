using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Grammar.UnitTests
{
    public class RecursiveGrammar_UnitTests
    {
        [Theory, RecursiveGrammarTestConvensions]
        public void CreateInstance_InstanceCreated()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                RecursiveGrammar sut = new RecursiveGrammar();
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }
    }
}
