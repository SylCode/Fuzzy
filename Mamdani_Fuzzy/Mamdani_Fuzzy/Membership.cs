using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamdani_Fuzzy
{
    class Membership
    {
        public double trimf (double x, double a, double b, double c)
        {
            if (x < a)
                return 0;
            else if (x >= a && x < b)
                return (x - a) / (b - a);
            else if (x >= b && x < c)
                return (c - x) / (c - b);
            else return 0;
        }

        public double trapmf(double x, double a, double b, double c, double d)
        {
            if (x < a)
                return 0;
            else if (x >= a && x < b)
                return (x - a) / (b - a);
            else if (x >= b && x < c)
                return 1;
            else if (x >= c && x < d)
                return (d - x) / (d - c);
            else return 0;
        }


        public double gaussmf(double x, double a, double c)
        {
            return Math.Exp(-((Math.Pow(x - a, 2) / (2 * c * c))));
        }
    }
}
