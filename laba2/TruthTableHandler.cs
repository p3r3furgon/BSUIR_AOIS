
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
            for (int i = 0; i < vars.Count; i++)
                Console.Write(vars[i] + "\t");
            Console.WriteLine("expr\tFi");
            for (int i = 0; i < truthTable.Count; i++)
            {
                for (int j = 0; j < truthTable[i].Count; j++)
                {
                    Console.Write(truthTable[i][j] ? "1\t" : "0\t");
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

        public static void PrintPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            StringBuilder disjuctionFunction = new StringBuilder();
            List<string> functionParts = CalculatingPartsOfPDNF(truthTable, vars, expressionResult);
            if (functionParts.Count != 0)
            {
                disjuctionFunction.Append(functionParts[0]);
                for (int i = 1; i < functionParts.Count; i++)
                    disjuctionFunction.Append("+" + functionParts[i]);
                Console.WriteLine("Principal disjunction normal function:\n" + disjuctionFunction.ToString() + "\n");
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

        public static void PrintPCNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            StringBuilder disjuctionFunction = new StringBuilder();
            List<string> functionParts = CalculatingPartsOfPCNF(truthTable, vars, expressionResult);
            if (functionParts.Count != 0)
            {
                disjuctionFunction.Append(functionParts[0]);
                for (int i = 1; i < functionParts.Count; i++)
                    disjuctionFunction.Append("*" + functionParts[i]);
                Console.WriteLine("Principal conjuction normal function:\n" + disjuctionFunction.ToString() + "\n");
            }
            else
                Console.WriteLine("Principal conjuction normal function doesn't exist\n");
        }

        public static void NumericInterpretationOfPDNF(List<List<bool>> truthTable, List<bool> expressionResult)
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
            if (results.Count != 0)
            {
                Console.WriteLine("PDNF numeric form:");
                string result = "+(" + results[0];
                for (int i = 1; i < results.Count; i++)
                    result += ", " + results[i];
                Console.WriteLine(result + ")\n");
            }
        }

        public static void NumericInterpretationOfPCNF(List<List<bool>> truthTable, List<bool> expressionResult)
        {
            List<List<bool>> suitableOptions = new List<List<bool>>();
            List<int> results = new List<int>();
            for (int i = 0; i < expressionResult.Count; i++)
            {
                if (!expressionResult[i])
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
            if (results.Count != 0)
            {
                Console.WriteLine("PCNF numeric form:");
                string result = "*(" + results[0];
                for (int i = 1; i < results.Count; i++)
                    result += ", " + results[i];
                Console.WriteLine(result + ")\n");
            }
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

        public static void IndexInterpretation(List<bool> expressionResult)
        {
            int index = 0;
            for (int i = 0; i < expressionResult.Count; i++)
            {
                if (expressionResult[i])
                    index += (int)Math.Pow(2, expressionResult.Count-1-i); 
            }
            Console.WriteLine("\nIndex Form:\ni = " + index);
        }

        public static void ShowPDNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            PrintPDNF(truthTable, vars, expressionResult);
            NumericInterpretationOfPDNF(truthTable, expressionResult);
        }

        public static void ShowPCNF(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            PrintPCNF(truthTable, vars, expressionResult);
            NumericInterpretationOfPCNF(truthTable, expressionResult);           
        }

        public static void PrintTotalResults(List<List<bool>> truthTable, List<string> vars, List<bool> expressionResult)
        {
            ShowPDNF(truthTable, vars, expressionResult);
            Console.WriteLine();
            ShowPCNF(truthTable, vars, expressionResult);
            IndexInterpretation(expressionResult);
        }
    }
}