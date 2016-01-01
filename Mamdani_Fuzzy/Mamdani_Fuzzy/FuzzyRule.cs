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
        EqualityComparer eq;

        public FuzzyRules()
        {
            rules = new Dictionary<string[], string>(new EqualityComparer());
        }
        public void addRule (string[] conditions, string result)
        {
            rules.Add(conditions, result);
        }

        public string checkRule(string[] conditions)
        {
            return rules[conditions];
        }
    }
}
