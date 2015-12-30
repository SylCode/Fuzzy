using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamdani_Fuzzy
{
    class DeFuzzySet
    {
        double[] data;
        double[,] membership;

        public DeFuzzySet(double[] dat)
        {
            data = dat;
            membership = new double[data.Length,3];
        }

        public DeFuzzySet(double[] dat, double[,] member)
        {
            data = dat;
            membership = member;
        }
    }
}
