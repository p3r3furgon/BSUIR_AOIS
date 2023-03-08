
using System.Collections.Generic;
using System;
using System.Text;
using Microsoft.SqlServer.Server;

namespace AOIS_2
{
    internal static class TruthTableHandler
    {
        public static List<List<bool>> Permutation(int n)
        {
            List<List<bool>> truthTable = new List<List<bool>>();
            List<bool> firstPermutation = new List<bool>();
            for (int i = 0; i < n; i++)
            {
                firstPermutation.Add(false);
            }
            truthTable.Add(firstPermutation);
            for (; ; )
            {
                int iter = firstPermutation.Count - 1;
                List<bool> currentPermutation = new List<bool>(truthTable[truthTable.Count - 1]);
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
                truthTable.Add(currentPermutation);
            }
            return truthTable;
        }

        public static void PrintTruthTable(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            for(int i = 0; i < vars.Count; i++)
                Console.Write(vars[i] + "\t");
            Console.WriteLine("expression");
            for (int i = 0; i < truthTable.Count; i++)
            {
                for (int j = 0; j < truthTable[i].Count; j++)
                {
                    Console.Write(truthTable[i][j] ? "1\t" : "0\t");
                }
                Console.Write(expressionResult[i] ? "1\t" : "0\t");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static List<string> CalculatingPartsOfPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            List<List<bool>> suitableOptions = new List<List<bool>>();
            List<string> functionParts = new List<string>();
            for(int i = 0; i < expressionResult.Count; i++)
            {
                if (expressionResult[i])
                    suitableOptions.Add(truthTable[i]);
            }
            for (int i = 0; i < suitableOptions.Count; i++)
            {
                StringBuilder functionPart = new StringBuilder("(");
                for(int j = 0; j < vars.Count; j++)
                {
                    if (suitableOptions[i][j])
                        functionPart.Append(vars[j] + "*");
                    else
                        functionPart.Append("(!" + vars[j] + ")*");
                }
                functionPart.Remove(functionPart.Length-1,1);
                functionPart.Append(")");
                functionParts.Add(functionPart.ToString());
            }
            return functionParts;
        }

        public static void PrintPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            StringBuilder disjuctionFunction = new StringBuilder();
            List<string> functionParts = CalculatingPartsOfPDNF(truthTable, vars, expressionResult);
            if (functionParts.Count != 0)
            {
                disjuctionFunction.Append(functionParts[0]);
                for (int i = 1; i < functionParts.Count; i++)
                    disjuctionFunction.Append("+" + functionParts[i]);
                Console.WriteLine("Principal disjunction normal function:\n\n" + disjuctionFunction.ToString() + "\n");
            }
            else
                Console.WriteLine("Principal disjunction normal function doesn't exist\n\n");
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

        public static void PrintPCNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            StringBuilder disjuctionFunction = new StringBuilder();
            List<string> functionParts = CalculatingPartsOfPCNF(truthTable, vars, expressionResult);
            if (functionParts.Count != 0)
            {
                disjuctionFunction.Append(functionParts[0]);
                for (int i = 1; i < functionParts.Count; i++)
                    disjuctionFunction.Append("*" + functionParts[i]);
                Console.WriteLine("Principal conjuction normal function:\n\n" + disjuctionFunction.ToString() + "\n");
            }
            else
                Console.WriteLine("Principal conjuction normal function doesn't exist\n\n");
        }
    }
}