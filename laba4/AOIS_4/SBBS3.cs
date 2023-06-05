using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_4
{
    internal class SBBS3
    {
        const int inputCount = 3;
        static string[] inputVariables = { "X1", "X2", "X3" };
        static string[] outputVariables = { "D", "B" };
        static bool DetermineDifferenceValue(bool minuend, bool subtrahend, bool borrowIn)
        {
            return minuend ^ subtrahend ^ borrowIn;
        }

        static bool DetermineBorrowsValue(bool minuend, bool subtrahend, bool borrowIn)
        {
            return (!minuend && subtrahend) || (!minuend && borrowIn) || (subtrahend && borrowIn);
        }

        static List<bool> GetDifferences()
        {
            List<bool> differences = new List<bool>();
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            foreach (var permutation in permutations)
            {
                differences.Add(DetermineDifferenceValue(permutation[0], permutation[1], permutation[2]));
            }
            return differences;
        }

        static List<bool> GetBorrows()
        {
            List<bool> borrows = new List<bool>();
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            foreach (var permutation in permutations)
            {
                borrows.Add(DetermineBorrowsValue(permutation[0], permutation[1], permutation[2]));
            }
            return borrows;
        }

        public static void PrintSBBS3TruthTable()
        {
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            List<bool> differences = GetDifferences();
            List<bool> borrows = GetBorrows();
            Console.WriteLine("SBBS3 truth table:\n");
            for (int i = 0; i < inputVariables.Length; i++)
                Console.Write(inputVariables[i] + "\t");
            for (int i = 0; i < outputVariables.Length; i++)
                Console.Write(outputVariables[i] + "\t");
            Console.WriteLine();
            for (int i = 0; i < permutations.Count; i++)
            {
                for (int j = 0; j < permutations[i].Count; j++)
                {
                    Console.Write(permutations[i][j] ? "1\t" : "0\t");
                }
                Console.Write(differences[i] ? "1\t" : "0\t");
                Console.Write(borrows[i] ? "1\t" : "0\t");
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        static void PrintDifferencePDNF()
        {
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            List<bool> differences = GetDifferences();
            TruthTableHandler.PrintPDNF(permutations, inputVariables.ToList(), differences);
        }

        static void PrintDifferenceMDNF()
        {
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            List<bool> differences = GetDifferences();
            LogicalExpression expression = new LogicalExpression(permutations, inputVariables.ToList(), differences);
            FunctionMinimizationHandler.PrintMDNF(expression);
        }

        static void PrintBorrowsPDNF()
        {
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            List<bool> borrows = GetBorrows();
            TruthTableHandler.PrintPDNF(permutations, inputVariables.ToList(), borrows);
        }

        static void PrintBorrowsMDNF()
        {
            List<List<bool>> permutations = TruthTableHandler.Permutation(inputCount);
            List<bool> borrows = GetBorrows();
            LogicalExpression expression = new LogicalExpression(permutations, inputVariables.ToList(), borrows);
            FunctionMinimizationHandler.PrintMDNF(expression);
        }

        public static void PrintTotalInformation()
        {
            PrintSBBS3TruthTable();
            Console.WriteLine("Difference:\n");
            PrintDifferencePDNF();
            PrintDifferenceMDNF();
            Console.WriteLine("\nBorrows:\n");
            PrintBorrowsPDNF();
            PrintBorrowsMDNF();
        }
    }
}
