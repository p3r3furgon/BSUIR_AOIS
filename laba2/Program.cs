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
            Dictionary<string, bool> variablesValues = new Dictionary<string, bool>();
            List<bool> expressionResult = new List<bool>();
            Stack<string> stackSigns = new Stack<string>();
            Stack<bool> stackVars = new Stack<bool>();
            List<string> tokens = new List<string>();
            List<string> vars = new List<string>();
            List<string> allVars = new List<string>();
            CountingVariables(logicExpression, vars, allVars);
            DividingExpressionOnTokens(logicExpression, tokens, allVars);
            int numberOfVariebles = vars.Count, numberOfPermutations = (int)Math.Pow(2, numberOfVariebles);
            var truthTable = TruthTableHandler.Permutation(numberOfVariebles);
            for(int i = 0; i < numberOfPermutations; i++)
            {
                for (int j = 0; j < numberOfVariebles; j++)
                    variablesValues[vars[j]] = truthTable[i][j];
                expressionResult.Add(LogicCalculator.Calculating(tokens, vars, variablesValues, stackSigns, stackVars));
            }            
            TruthTableHandler.PrintTruthTable(truthTable, vars, expressionResult);

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
    }
}
