using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_4
{
    internal class D8421
    {
        static string[] inputVariables = { "X1", "X2", "X3", "X4" };
        static string[] outputVariables = { "Y1", "Y2", "Y3", "Y4" };
        const int n = 4;
        const int shiftValue = 4;

        public static List<List<bool>> BuildGlobalTruthTable()
        {
            var xPermutations = TruthTableHandler.Permutation(n);
            var yPermutations = BuildYTruthTable(shiftValue, n);
            List<List<bool>> globalTruthTable = new List<List<bool>>(xPermutations);
            for(int i = 0; i < yPermutations.Count; i++)
            {
                for(int j = 0; j < yPermutations[i].Count; j++)
                {
                    globalTruthTable[i].Add(yPermutations[i][j]);
                }
            }
            return globalTruthTable;
        }

        public static List<List<bool>> BuildYTruthTable(int shiftValue, int n)
        {
            List<List<bool>> yPermutation = new List<List<bool>>();
            List<List<bool>> normalPermutation = TruthTableHandler.Permutation(n);
            int counter = 0, numOfOperations = 10;
            int i = shiftValue;
            while(true)
            {
                if (i == normalPermutation.Count)
                    i = 0;
                if(counter == numOfOperations)
                    break;
                yPermutation.Add(normalPermutation[i]);
                i++;
                counter++;
            }
            return yPermutation;
        }

        public static void PrintGlobalTruthTable()
        {
            var globalTruthTable = BuildGlobalTruthTable();
            Console.WriteLine("D8421 Truth Table\n");
            foreach(var inpVar in inputVariables)
            {
                Console.Write(inpVar + "\t");
            }
            foreach (var outVar in outputVariables)
            {
                Console.Write(outVar + "\t");
            }
            Console.WriteLine();
            foreach (var row in globalTruthTable)
            {
                foreach(var element in row)
                {
                    Console.Write(element? "1\t" : "0\t");
                }
                Console.WriteLine();
            }
        }

        public static void FindMinFormula()
        {
            var yPermutations = BuildYTruthTable(shiftValue, n);
            for(int i = 0; i < n; i++)
            {
                var MDNF = ShortestMDNF(i, yPermutations);
                var MCNF = ShortestMCNF(i, yPermutations);
                string minFormula = MDNF.Length <= MCNF.Length ? MDNF : MCNF; 
                Console.WriteLine("Y" + (i+1).ToString() + ":\n" + MDNF);
            }
        }

        public static string ShortestMDNF(int colIndex, List<List<bool>> yPermutations)
        {
            List<bool> yCol = new List<bool>();
            List<List<bool>> permutations = TruthTableHandler.Permutation(4);  
            List<string> allMDNFs = new List<string>();
            List<string> allMCNFs = new List<string>();
            for(int j = 0; j < yPermutations.Count; j++)
            {
                yCol.Add(yPermutations[j][colIndex]);
            }
            var addictivePermutations = TruthTableHandler.Permutation(6);
            for (int j = 0; j < addictivePermutations.Count; j++)
            {
                var yColTmp = yCol.Concat(addictivePermutations[j]).ToList();
                var expr = new LogicalExpression(permutations, inputVariables.ToList(), yColTmp);
                allMDNFs.Add(FunctionMinimizationHandler.GetStringMDNF(expr));
            }
            int minLength = int.MaxValue;
            string shortestMDNF = "";
            foreach(var MDNF in allMDNFs)
            {
                if(MDNF.Length < minLength)
                {
                    minLength = MDNF.Length;
                    shortestMDNF = MDNF;
                }
            }
            return shortestMDNF;
        }

        public static string ShortestMCNF(int colIndex, List<List<bool>> yPermutations)
        {
            List<bool> yCol = new List<bool>();
            List<List<bool>> permutations = TruthTableHandler.Permutation(4);
            List<string> allMCNFs = new List<string>();
            for (int j = 0; j < yPermutations.Count; j++)
            {
                yCol.Add(yPermutations[j][colIndex]);
            }
            var addictivePermutations = TruthTableHandler.Permutation(6);
            for (int j = 0; j < addictivePermutations.Count; j++)
            {
                var yColTmp = yCol.Concat(addictivePermutations[j]).ToList();
                var expr = new LogicalExpression(permutations, inputVariables.ToList(), yColTmp);
                allMCNFs.Add(FunctionMinimizationHandler.GetStringMCNF(expr));
            }
            int minLength = int.MaxValue;
            string shortestMCNF = "";
            foreach (var MCNF in allMCNFs)
            {
                if (MCNF.Length < minLength)
                {
                    minLength = MCNF.Length;
                    shortestMCNF = MCNF;
                }
            }
            return shortestMCNF;
        }
    }

}
