using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamdani_Fuzzy
{
    class FuzzySet
    {
        public Dictionary<string, double[]> Set;

        public FuzzySet()
        {
            Set = new Dictionary<string, double[]>();
        }
        
    }
}
