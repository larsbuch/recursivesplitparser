using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammar.UnitTests
{
    public class RecursiveGrammarTestConvensionsAttribute : AutoDataAttribute
    {
        public RecursiveGrammarTestConvensionsAttribute(): base(new Fixture().Customize(new AutoMoqCustomization()))
        {
            this.Fixture.Customize(new OmitAutoPropertiesForTypesInNamespace("RecursiveGrammar"));
        }
    }
}
