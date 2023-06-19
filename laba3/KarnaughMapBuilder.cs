using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_3
{
    internal static class KarnaughMapBuilder
    {

        public static List<List<int>> GetGrayCode(int  n)
        {
            List<List<int>> grayCode = new List<List<int>>();
            List<int> startRow = new List<int>();
            for(int i =0; i < n+1; i++)
            {
                startRow.Add(0);
            }
            int j = 0;
            for (; ; )
            {
                List<int> grayCodeRow = startRow.GetRange(0, n);
                grayCodeRow.Reverse();
                grayCode.Add(grayCodeRow);
                startRow[n] = 1 - startRow[n];
                if (startRow[n] == 1)
                    j = 0;
                else
                {
                    for (j = 0; j < n; j++)
                    {
                        if (startRow[j] == 1)
                        {
                            j += 1;
                            break;
                        }
                    }
                }
                if (j == n)
                    return grayCode;
                startRow[j] = 1 - startRow[j];
            }
        }

        public static Dictionary<string, int> ExpressionResultIntoDictionaryForm(LogicalExpression expression)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var permutations = expression.VarsPermutations;
            var resultVector = expression.ExpressionResult;
            for(int i = 0; i < permutations.Count; i++)
            {
                string strPermutation = "";
                for(int j = 0; j < permutations[i].Count; j++)
                {
                    strPermutation += permutations[i][j] ? "1" : "0";
                }
                int permutationResult = resultVector[i] ? 1 : 0; 
                result.Add(strPermutation, permutationResult);
            }
            return result; 
        }

        public static List<List<int>> GetKarnaughMap(LogicalExpression expression)
        {
            var grayCodeTable = new List<List<int>>();
            var grayCode = GetGrayCode(expression.NumberOfVars);
            var expressionResults = ExpressionResultIntoDictionaryForm(expression);
            int a = grayCode[0].Count / 2;
            int b = grayCode[0].Count - a;
            int counter = 0;
            for(int i = 0; i< Math.Pow(2, a); i ++)
            {
                var tableRow = new List<int>();
                for(int j = 0; j < Math.Pow(2, b); j++)
                {
                    string strGrayCodeRow = string.Join("", grayCode[counter]);
                    tableRow.Add(expressionResults[strGrayCodeRow]);
                    counter++;
                }
                if (i % 2 == 1)
                    tableRow.Reverse();
                grayCodeTable.Add(tableRow); 
            }
            grayCodeTable.Reverse();
            return grayCodeTable;
        }

        public static void PrintKarnaughMap(LogicalExpression expression, bool isPDNF)
        {
            var KarnaughtMap = GetKarnaughMap(expression);
            var grayCode = GetGrayCode(expression.NumberOfVars);
            var variables = expression.UniqeVars;
            int a = grayCode[0].Count / 2;
            int b = grayCode[0].Count - a;
            var yVector = GetGrayCode(a);
            yVector.Reverse();
            var xVector = GetGrayCode(b);
            var yVars = variables.Take(a);
            var xVars = variables.GetRange(a,variables.Count-a);
            Console.WriteLine(String.Join(", ", yVars) + "\n");

            for (int i = 0; i < Math.Pow(2, a); i++)
            {
                Console.Write(String.Join("", yVector[i]) + "\t");
                for (int j = 0; j < Math.Pow(2, b); j++)
                {
                    Console.Write(KarnaughtMap[i][j] + "\t");
                }
                Console.WriteLine();
            }
            Console.Write("\n\t");
            for(int i = 0; i < Math.Pow(2, b); i++)
            {
                Console.Write(String.Join("", xVector[i]) + "\t");
            }
            Console.WriteLine(String.Join(", ", xVars) + "\n");
        }
    }
}
