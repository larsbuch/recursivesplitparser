﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammar
{
    public class Production
    {
        private ProductionPart _firstProductionPart;
        public string ProductionName { get; private set; }
        private ProductionPart _activeProductionPart;

        public Production(ProductionPart productionPartLinkedList)
        {
            _firstProductionPart = productionPartLinkedList;
            _activeProductionPart = productionPartLinkedList;
        }

        public void ResetProduction()
        {
            _activeProductionPart = _firstProductionPart;
        }

        public bool MatchToken(Token token)
        {
            // if match move forward
            throw new NotImplementedException();
        }
    }
}