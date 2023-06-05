using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOIS_3
{
    internal static class LogicCalculator
    {
        public static bool Calculating(List<string> tokens, List<string> vars, Dictionary<string, bool> values)
        {
            Stack<bool> stackVars = new Stack<bool>();
            Stack<string> stackSigns = new Stack<string>();
            string[] signs = { "!", "*", "+", "->", "==" };
            foreach (var token in tokens)
            {
                if (vars.Contains(token))
                    stackVars.Push(values[token]);
                else if (signs.Contains(token))
                {
                    while (stackSigns.Count >= 1 && Priority.GetPriority(stackSigns.Peek()) >= Priority.GetPriority(token))
                        stackVars.Push(ExecuteOperation(stackVars, stackSigns.Pop()));
                    stackSigns.Push(token);
                }
                else
                {
                    if (token == "(")
                        stackSigns.Push(token);
                    else
                    {
                        while (stackSigns.Peek() != "(")
                            stackVars.Push(ExecuteOperation(stackVars, stackSigns.Pop()));
                        stackSigns.Pop();
                    }
                }
            }
            while (stackSigns.Count != 0)
                stackVars.Push(ExecuteOperation(stackVars, stackSigns.Pop()));
            return stackVars.Pop();
        }

        static bool ExecuteBinaryOperation(Stack<bool> stackVars, string sign)
        {
            bool value2 = stackVars.Pop();
            bool value1 = stackVars.Pop();
            switch(sign)
            {
                case "*":
                    return Conjunction(value1, value2);
                case "+":
                    return Disjunction(value1, value2);
                case "->":
                    return Implication(value1, value2);
                default:
                    return Equivalence(value1, value2);
            }          
        }

        static bool ExecuteOperation(Stack<bool> stackVars, string sign)
        {
            if (sign == "!")
                return Inversion(stackVars.Pop());
            else
                return ExecuteBinaryOperation(stackVars, sign);
        }

        static bool Conjunction(bool x1, bool x2)
        {
            return x1 && x2;
        }

        static bool Disjunction(bool x1, bool x2)
        {
            return x1 || x2;
        }

        static bool Inversion(bool x1)
        {
            return !x1;
        }

        static bool Implication(bool x1, bool x2)
        {
            return !(x1 == true && x2 == false);
        }

        static bool Equivalence(bool x1, bool x2)
        {
            return x1 == x2;
        }
    }

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
            if (priorities.ContainsKey(sign))
                return priorities[sign];
            else
                return 0;
        }
    }
}