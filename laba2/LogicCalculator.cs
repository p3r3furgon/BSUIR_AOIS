﻿using System.Collections.Generic;
using System.Linq;

namespace AOIS_2
{

    public static class LogicCalculator
    {
        public static bool Calculating(List<string> tokens, List<string> vars, Dictionary<string, bool> values, Stack<string> stackSigns, Stack<bool> stackVars)
        {
            string[] signs = { "!", "*", "+", "->", "#" };
            foreach (var token in tokens)
            {
                if (vars.Contains(token))
                    stackVars.Push(values[token]);
                else if (signs.Contains(token))
                {
                    while (stackSigns.Count >= 1 && Priorety(stackSigns.Peek()) >= Priorety(token))
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
            if (sign == "*")
                return Conjunction(value1, value2);
            else if (sign == "+")
                return Disjunction(value1, value2);
            else if (sign == "->")
                return Implication(value1, value2);
            else
                return Equivalence(value1, value2);
        }

        static bool ExecuteOperation(Stack<bool> stackVars, string sign)
        {
            if (sign == "!")
                return Inversion(stackVars.Pop());
            else
                return ExecuteBinaryOperation(stackVars, sign);
        }

        static int Priorety(string sign)
        {
            if (sign == "!")
                return 5;
            else if (sign == "*")
                return 4;
            else if (sign == "+")
                return 3;
            else if (sign == "->")
                return 2;
            else
                return 1;
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
            if (x1 == true && x2 == false)
                return false;
            else
                return true;
        }

        static bool Equivalence(bool x1, bool x2)
        {
            return x1 == x2;
        }
    }
}