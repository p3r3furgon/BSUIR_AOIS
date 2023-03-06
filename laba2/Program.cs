using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_2{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the logic statement:");
            string logicExpression = Console.ReadLine();
            Dictionary<string, bool> values = new Dictionary<string, bool>();
            List<string> tokens = new List<string>();
            List<string> vars = new List<string>();
            List<string> allVars = new List<string>();
            Stack<string> stackSigns = new Stack<string>();
            Stack<bool> stackVars = new Stack<bool>();
            //var s = logicExpression.Split('-', '>',')','(','+','*','!','#');
            CountingVariables(logicExpression, vars, allVars);
            int numberOfVariebles = vars.Count;
            values[vars[0]] = false;
            values[vars[1]] = false;
            values[vars[2]] = true;
            DividingExpressionOnTokens(logicExpression, tokens, allVars);
            Console.WriteLine(Calculating(tokens, vars, values, stackSigns, stackVars).ToString());
            
        }

        static void CountingVariables(string expression, List<string> vars, List<string> allVars)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if ((expression[i] >= 65 && expression[i] <= 90) || (expression[i] >= 97 && expression[i] <= 122))
                {
                    int varSize = 1;
                    while (i+varSize < expression.Length && char.IsDigit(expression[i+ varSize]))
                        varSize++;
                    if (!vars.Contains(expression.Substring(i, varSize)))
                        vars.Add(expression.Substring(i, varSize));
                    allVars.Add(expression.Substring(i, varSize));
                }
            }
        }

        static void DividingExpressionOnTokens(string expression, List<string> tokens, List<string> allVars)
        {
            int varNumber = 0;
            for(int i = 0; i < expression.Length; i++)
            {
                if ((expression[i] >= 65 && expression[i] <= 90) || (expression[i] >= 97 && expression[i] <= 122))
                {
                    tokens.Add(allVars[varNumber]);
                    i += (allVars[varNumber].Length - 1);
                    varNumber++;
                }
                else
                {
                    if (expression[i] == '-' && expression[i + 1] == '>')
                    {
                        tokens.Add(expression.Substring(i, 2));
                        i++;
                    }
                    else
                        tokens.Add(expression[i].ToString());
                }
            }
        }

        static bool Calculating(List<string> tokens, List<string> vars, Dictionary<string,bool> values, Stack<string> stackSigns, Stack<bool> stackVars)
        {
            string[] signs = { "!", "*", "+", "->", "#" };
            foreach (var token in tokens)
            {
                if (vars.Contains(token))
                    stackVars.Push(values[token]);
                else if (signs.Contains(token))
                {
                    while(stackSigns.Count >= 1 && Priorety(stackSigns.Peek()) >= Priorety(token))
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
            else if(sign == "->")
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
