using AOIS_1;
using AOIS_4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_5
{
    internal class Converter
    {
        static string[] inpPreTactNames = { "q01", "q02", "q03", "q04" };
        static string[] inpPostTactNames = { "q11", "q12", "q13", "q14" };
        static string[] triggerNames = { "h1", "h2", "h3", "h4" };
        static string fsmName = "V";
        const int numOfElements = 5;

        public static List<int> BinaryValueParse(string binaryValue)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < binaryValue.Length; i++)
                result.Add(binaryValue[i] == '1'? 1:0);
            return result;
        }

        public static List<List<int>> BuildPostTactTable()
        { 
            List<List<int>> postTactTable = new List<List<int>>();
            var permutations = BuildIntTablePermutations();
            foreach(var permutation in permutations)
            {
                string preTactValue = permutation[0].ToString() + permutation[1].ToString() + permutation[2].ToString() + permutation[3].ToString();
                string fsmValue = permutation[4].ToString();
                int intPostTactValue = BinaryDecimalConverter.ConvertPositiveBinaryValue(preTactValue) 
                    - BinaryDecimalConverter.ConvertPositiveBinaryValue(fsmValue);
                string strPostTactValue = "";
                if (intPostTactValue == -1)
                {
                    strPostTactValue = BinaryDecimalConverter.ConvertToBinary(15, 4); ;
                }
                else
                {
                    strPostTactValue = BinaryDecimalConverter.ConvertToBinary(intPostTactValue, 4);
                }
                var postTactRow = BinaryValueParse(strPostTactValue);
                postTactTable.Add(postTactRow);
            }
            return postTactTable;
        }

        public static List<List<int>> BuildIntTablePermutations()
        {
            List<List<int>> permutations = new List<List<int>>();
            List<int> firstPermutation = new List<int>();
            for (int i = 0; i < numOfElements; i++)
            {
                firstPermutation.Add(0);
            }
            permutations.Add(firstPermutation);
            for (; ; )
            {
                int iter = firstPermutation.Count - 1;
                List<int> currentPermutation = new List<int>(permutations[permutations.Count - 1]);
                for (; iter >= 0; iter--)
                    if (currentPermutation[iter] == 0)
                        break;
                if (iter < 0)
                    break;
                currentPermutation[iter] = 1;
                iter++;
                for (; iter < numOfElements;)
                {
                    currentPermutation[iter] = 0;
                    iter++;
                }
                permutations.Add(currentPermutation);
            }
            return permutations;
        }

        public static List<List<int>> BuildEcxitationTable()
        {
            var excitationTable = new List<List<int>>();
            var postTactTable = BuildPostTactTable();
            var permutations = BuildIntTablePermutations();
            for(int i = 0; i < postTactTable.Count; i ++)
            {
                var excitationTableRow = new List<int>();
                for (int j = 0; j < postTactTable[i].Count; j++)
                {
                    excitationTableRow.Add(postTactTable[i][j] == permutations[i][j] ? 0 : 1);
                }
                excitationTable.Add(excitationTableRow);
            }
            return excitationTable;
        }
        public static void PrintGlobalTable() 
        {
            var permutations = BuildIntTablePermutations();
            var postTactTable = BuildPostTactTable();
            var excitationTable = BuildEcxitationTable();
            for (int i = 0; i < 4; i++)
                Console.Write(inpPreTactNames[i] + "\t");
            Console.Write(fsmName + "\t");
            for (int i = 0; i < 4; i++)
                Console.Write(inpPostTactNames[i] + "\t");
            for (int i = 0; i < 4; i++)
                Console.Write(triggerNames[i] + "\t");
            Console.WriteLine();
            for (int i = 0; i < permutations.Count; i++)
            {
                for (int j = 0; j < permutations[i].Count; j++)
                {
                    Console.Write(permutations[i][j] + "\t");
                }
                for (int j = 0; j < postTactTable[i].Count; j++)
                {
                    Console.Write(postTactTable[i][j] + "\t");
                }
                for (int j = 0; j < excitationTable[i].Count; j++)
                {
                    Console.Write(excitationTable[i][j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");
        }

        public static void FindMinFormula()
        {
            var excitationTable = BuildEcxitationTable();
            for (int i = 0; i < 4; i++)
            {
                var MDNF = ShortestMDNF(i, ToBoolList(excitationTable));
                var MCNF = ShortestMCNF(i, ToBoolList(excitationTable));
                string minFormula = MDNF.Length <= MCNF.Length ? MDNF : MCNF;
                Console.WriteLine("h" + (i + 1).ToString() + ":\n" + minFormula);
            }
        }

        public static List<List<bool>> ToBoolList(List<List<int>> intList)
        {
            List<List<bool>> boolList = new List<List<bool>>();
            foreach(var intListRow in intList)
            {
                List<bool> boolListRow = new List<bool>();
                foreach (var el in intListRow)
                {
                    boolListRow.Add(el == 1 ? true : false);
                }
                boolList.Add(boolListRow);
            }
            return boolList;
        }

        public static string ShortestMDNF(int colIndex, List<List<bool>> excitationTable)
        {
            List<bool> excitationTableRow = new List<bool>();
            List<List<bool>> permutations = TruthTableHandler.Permutation(numOfElements);
            List<string> allMDNFs = new List<string>();
            for (int j = 0; j < excitationTable.Count; j++)
            {
                excitationTableRow.Add(excitationTable[j][colIndex]);
            }
            var inputVariables = inpPreTactNames.ToList();
            inputVariables.Add(fsmName);
            var expr = new LogicalExpression(permutations, inputVariables, excitationTableRow);
            allMDNFs.Add(FunctionMinimizationHandler.GetStringMDNF(expr));

            int minLength = int.MaxValue;
            string shortestMDNF = "";
            foreach (var MDNF in allMDNFs)
            {
                if (MDNF.Length < minLength)
                {
                    minLength = MDNF.Length;
                    shortestMDNF = MDNF;
                }
            }
            return shortestMDNF;
        }

        public static string ShortestMCNF(int colIndex, List<List<bool>> excitationTable)
        {
            List<bool> excitationTableRow = new List<bool>();
            List<List<bool>> permutations = TruthTableHandler.Permutation(numOfElements);
            List<string> allMCNFs = new List<string>();
            for (int j = 0; j < excitationTable.Count; j++)
            {
                excitationTableRow.Add(excitationTable[j][colIndex]);
            }
            var inputVariables = inpPreTactNames.ToList();
            inputVariables.Add(fsmName);
            var expr = new LogicalExpression(permutations, inputVariables, excitationTableRow);
            allMCNFs.Add(FunctionMinimizationHandler.GetStringMCNF(expr));

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
