using System.Collections.Generic;
using System;
using System.Text;
using Microsoft.SqlServer.Server;
using AOIS_3;

namespace AOIS_3
{

    internal static class TruthTableHandler
    {
        public static List<List<bool>> Permutation(int n)
        {
            List<List<bool>> varsPermutation = new List<List<bool>>();
            List<bool> firstPermutation = new List<bool>();
            for (int i = 0; i < n; i++)
            {
                firstPermutation.Add(false);
            }
            varsPermutation.Add(firstPermutation);
            for (; ; )
            {
                int iter = firstPermutation.Count - 1;
                List<bool> currentPermutation = new List<bool>(varsPermutation[varsPermutation.Count - 1]);
                for (; iter >= 0; iter--)
                    if (!currentPermutation[iter])
                        break;
                if (iter < 0)
                    break;
                currentPermutation[iter] = true;
                iter++;
                for (; iter < n;)
                {
                    currentPermutation[iter] = false;
                    iter++;
                }
                varsPermutation.Add(currentPermutation);
            }
            return varsPermutation;
        }

        public static void PrintTruthTable(List<List<bool>> varsPermutation, List<string> vars, List<bool> expressionResult)
        {
            for (int i = 0; i < vars.Count; i++)
                Console.Write(vars[i] + "\t");
            Console.WriteLine("expr\tFi");
            for (int i = 0; i < varsPermutation.Count; i++)
            {
                for (int j = 0; j < varsPermutation[i].Count; j++)
                {
                    Console.Write(varsPermutation[i][j] ? "1\t" : "0\t");
                }
                Console.Write(expressionResult[i] ? "1\t" : "0\t");
                Console.Write(i);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static List<string> CalculatingPartsOfPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            List<List<bool>> suitableOptions = new List<List<bool>>();
            List<string> functionParts = new List<string>();
            for (int i = 0; i < expressionResult.Count; i++)
            {
                if (expressionResult[i])
                    suitableOptions.Add(truthTable[i]);
            }
            for (int i = 0; i < suitableOptions.Count; i++)
            {
                StringBuilder functionPart = new StringBuilder("(");
                for (int j = 0; j < vars.Count; j++)
                {
                    if (suitableOptions[i][j])
                        functionPart.Append(vars[j] + "*");
                    else
                        functionPart.Append("(!" + vars[j] + ")*");
                }
                functionPart.Remove(functionPart.Length - 1, 1);
                functionPart.Append(")");
                functionParts.Add(functionPart.ToString());
            }
            return functionParts;



        }

        public static List<List<string>> CalculatingPartsOfPDNF(LogicalExpression logicExpression)
        {
            List<List<string>> suitableOptions = new List<List<string>>();
            for (int i = 0; i < logicExpression.ExpressionResult.Count; i++)
            {
                if (logicExpression.ExpressionResult[i])
                {
                    List<string> combination = new List<string>();
                    for (int j = 0; j < logicExpression.VarsPermutations[i].Count; j++)
                    {
                        if (logicExpression.VarsPermutations[i][j])
                            combination.Add(logicExpression.UniqeVars[j]);
                        else
                            combination.Add("!" + logicExpression.UniqeVars[j]);
                    }
                    suitableOptions.Add(combination);
                }
            }
            return suitableOptions;
        }

        public static void PrintPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            Console.WriteLine("Principal disjunction normal form:");
            StringBuilder disjuctionFunction = new StringBuilder();
            List<string> functionParts = CalculatingPartsOfPDNF(truthTable, vars, expressionResult);
            if (functionParts.Count != 0)
            {
                disjuctionFunction.Append(functionParts[0]);
                for (int i = 1; i < functionParts.Count; i++)
                    disjuctionFunction.Append("+" + functionParts[i]);
                Console.WriteLine(disjuctionFunction.ToString());
            }
            else
                Console.WriteLine("Principal disjunction normal function doesn't exist\n");
        }

        public static List<string> CalculatingPartsOfPCNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            List<List<bool>> suitableOptions = new List<List<bool>>();
            List<string> functionParts = new List<string>();
            for (int i = 0; i < expressionResult.Count; i++)
            {
                if (!expressionResult[i])
                    suitableOptions.Add(truthTable[i]);
            }
            for (int i = 0; i < suitableOptions.Count; i++)
            {
                StringBuilder functionPart = new StringBuilder("(");
                for (int j = 0; j < vars.Count; j++)
                {
                    if (!suitableOptions[i][j])
                        functionPart.Append(vars[j] + "+");
                    else
                        functionPart.Append("(!" + vars[j] + ")+");
                }
                functionPart.Remove(functionPart.Length - 1, 1);
                functionPart.Append(")");
                functionParts.Add(functionPart.ToString());
            }
            return functionParts;
        }

        public static List<List<string>> CalculatingPartsOfPCNF(LogicalExpression logicExpression)
        {
            List<List<string>> suitableOptions = new List<List<string>>();
            for (int i = 0; i < logicExpression.ExpressionResult.Count; i++)
            {
                if (!logicExpression.ExpressionResult[i])
                {
                    List<string> combination = new List<string>();
                    for (int j = 0; j < logicExpression.VarsPermutations[i].Count; j++)
                    {
                        if (!logicExpression.VarsPermutations[i][j])
                            combination.Add(logicExpression.UniqeVars[j]);
                        else
                            combination.Add("!" + logicExpression.UniqeVars[j]);
                    }
                    suitableOptions.Add(combination);
                }
            }
            return suitableOptions;
        }

        public static void PrintPCNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            StringBuilder disjuctionFunction = new StringBuilder();
            List<string> functionParts = CalculatingPartsOfPCNF(truthTable, vars, expressionResult);
            Console.WriteLine("Principal conjuction normal form:");
            if (functionParts.Count != 0)
            {
                disjuctionFunction.Append(functionParts[0]);
                for (int i = 1; i < functionParts.Count; i++)
                    disjuctionFunction.Append("*" + functionParts[i]);
                Console.WriteLine(disjuctionFunction.ToString());
            }
            else
                Console.WriteLine("Principal conjuction normal function doesn't exist\n");
        }
        public static void PrintNumericInterpretationOfPDNF(List<List<bool>> truthTable, List<bool> expressionResult)
        {
            List<int> partsOfPDNF = NumericInterpretationOfPDNF(truthTable, expressionResult);
            if (partsOfPDNF.Count != 0)
            {
                Console.WriteLine("\nPDNF numeric form:");
                string result = "+(" + partsOfPDNF[0];
                for (int i = 1; i < partsOfPDNF.Count; i++)
                    result += ", " + partsOfPDNF[i];
                Console.WriteLine(result + ")\n");
            }
        }

        public static List<int> NumericInterpretationOfPDNF(List<List<bool>> truthTable, List<bool> expressionResult)
        {
            List<List<bool>> suitableOptions = new List<List<bool>>();
            List<int> results = new List<int>();
            for (int i = 0; i < expressionResult.Count; i++)
            {
                if (expressionResult[i])
                    suitableOptions.Add(truthTable[i]);
            }
            for (int i = 0; i < suitableOptions.Count; i++)
            {
                string binaryValue = "";
                for (int j = 0; j < suitableOptions[i].Count; j++)
                {
                    if (suitableOptions[i][j])
                        binaryValue += "1";
                    else
                        binaryValue += "0";
                }
                results.Add(FromBinaryToInt(binaryValue));
            }
            return results;
        }

        public static void PrintNumericInterpretationOfPCNF(List<List<bool>> truthTable, List<bool> expressionResults)
        {
            List<int> partsOfPCNF = NumericInterpretationOfPCNF(truthTable, expressionResults);
            if (partsOfPCNF.Count != 0)
            {
                Console.WriteLine("\nPCNF numeric form:");
                string result = "+(" + partsOfPCNF[0];
                for (int i = 1; i < partsOfPCNF.Count; i++)
                    result += ", " + partsOfPCNF[i];
                Console.WriteLine(result + ")\n");
            }
        }

        public static List<int> NumericInterpretationOfPCNF(List<List<bool>> truthTable, List<bool> expressionResults)
        {
            List<List<bool>> suitableOptions = new List<List<bool>>();
            List<int> results = new List<int>();
            for (int i = 0; i < expressionResults.Count; i++)
            {
                if (!expressionResults[i])
                    suitableOptions.Add(truthTable[i]);
            }
            for (int i = 0; i < suitableOptions.Count; i++)
            {
                string binaryValue = "";
                for (int j = 0; j < suitableOptions[i].Count; j++)
                {
                    if (!suitableOptions[i][j])
                        binaryValue += "0";
                    else
                        binaryValue += "1";
                }
                results.Add(FromBinaryToInt(binaryValue));
            }
            return results;
        }

        public static int FromBinaryToInt(string binaryValue)
        {
            int result = 0;
            for (int i = 0; i < binaryValue.Length; ++i)
            {
                if (binaryValue[binaryValue.Length - i - 1] == '1')
                    result += (int)Math.Pow(2, i);
            }
            return result;
        }

        public static int IndexInterpretation(List<bool> expressionResults)
        {
            int index = 0;
            for (int i = 0; i < expressionResults.Count; i++)
            {
                if (expressionResults[i])
                    index += (int)Math.Pow(2, expressionResults.Count - 1 - i);
            }
            return index;
        }

        public static void ShowPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResults, LogicalExpression logicExpression)
        {
            PrintPDNF(truthTable, vars, expressionResults);
            PrintNumericInterpretationOfPDNF(truthTable, expressionResults);
            FunctionMinimizationHandler.PrintRDNF(logicExpression);

        }

        public static void ShowPCNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResults, LogicalExpression logicExpression)
        {
            PrintPCNF(truthTable, vars, expressionResults);
            PrintNumericInterpretationOfPCNF(truthTable, expressionResults);
            FunctionMinimizationHandler.PrintRCNF(logicExpression);
        }

        public static void PrintTotalResults(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResults, LogicalExpression logicExpression)
        {
            ShowPDNF(truthTable, vars, expressionResults, logicExpression);
            Console.WriteLine();
            ShowPCNF(truthTable, vars, expressionResults, logicExpression);

            Console.WriteLine("\nIndex form: " + IndexInterpretation(expressionResults));
        }
    }
}