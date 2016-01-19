using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser.UnitTests
{
    public class SplitParserTestConvensionsAttribute: AutoDataAttribute
    {
        public SplitParserTestConvensionsAttribute(): base(new Fixture().Customize(new AutoMoqCustomization()))
        {
            this.Fixture.Customize(new OmitAutoPropertiesForTypesInNamespace("SplitParser"));
        }
    }
}
