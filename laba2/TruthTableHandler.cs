
using System.Collections.Generic;
using System;

namespace AOIS_2
{
    public static class TruthTableHandler
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
            Console.WriteLine("res");
            for (int i = 0; i < truthTable.Count; i++)
            {
                for (int j = 0; j < truthTable[i].Count; j++)
                {
                    Console.Write(truthTable[i][j] ? "1\t" : "0\t");
                }
                Console.Write(expressionResult[i] ? "1\t" : "0\t");
                Console.WriteLine();
            }
        }
    }

}