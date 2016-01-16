using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mamdani_Fuzzy
{
    class FuzzyRules
    {
        Dictionary<string[], string> rules;
        Dictionary<string[], double> weight;
        EqualityComparer eq;

        public FuzzyRules()
        {
            rules = new Dictionary<string[], string>(new EqualityComparer());
            weight = new Dictionary<string[], double>(new EqualityComparer());
        }
        public void addRule (string[] conditions, string result, double w)
        {
            rules.Add(conditions, result);
            weight.Add(conditions, w);
        }

        public string checkRule(string[] conditions)
        {
            return rules[conditions];
        }

        public bool Contains (string[] conditions)
        {
            return rules.ContainsKey(conditions);
        }

        public void Replace (string[] conditions, string result, double w)
        {
            rules[conditions] = result;
            weight[conditions] = w;
        }

        public double getWeight(string[] conditions)
        {
            return weight[conditions];
        }


        public string[] getValues()
        {
            return rules.Values.ToArray();
        }

        public string[][] getKeys()
        {
            return rules.Keys.ToArray();
        }
    }
}
