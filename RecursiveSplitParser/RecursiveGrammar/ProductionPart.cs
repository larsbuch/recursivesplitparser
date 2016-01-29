using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    public class ProductionPart
    {
        public ProductionPart NextProductionPart { get; private set; }
        private Production _production;

        public ProductionPart(Production production)
        {
            _production = production;
        }
    }
}
