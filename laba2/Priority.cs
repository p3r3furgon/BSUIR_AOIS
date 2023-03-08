using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_2
{
    internal static class Priority
    {
        private static readonly Dictionary<string, int> priorities;
        
        static Priority()
        {
            priorities = new Dictionary<string, int>();
            priorities.Add("!", 5);
            priorities.Add("*", 4);
            priorities.Add("+", 3);
            priorities.Add("->", 2);
            priorities.Add("==", 1);
        }
        public static int GetPriority(string sign)
        {
            if(priorities.ContainsKey(sign))
                return priorities[sign];
            else 
                return 0;
        }
    }
    
}
