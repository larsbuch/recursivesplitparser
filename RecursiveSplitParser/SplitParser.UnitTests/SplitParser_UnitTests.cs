using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SplitParser.UnitTests
{
    public class SplitParser_UnitTests
    {
        [Theory, SplitParserTestConvensions]
        public void CreateInstance_InstanceCreated()
        {
            Exception expected = null;
            Exception actual = null;

            try
            {
                SplitParser sut = new SplitParser();
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }
    }
}
