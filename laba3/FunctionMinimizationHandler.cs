﻿using AOIS_3;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_3
{
    internal static class FunctionMinimizationHandler
    {
        static List<List<string>> CalculatingPartsOfRDNF(LogicalExpression logicExpression)
        {
            List<List<string>> partsOfPDNF = TruthTableHandler.CalculatingPartsOfPDNF(logicExpression);
            List<List<string>> singleParts = new List<List<string>>();
            while (CountingNumberOfVars(partsOfPDNF[0]) > 1)
            {
                List<List<string>> reducedParts = new List<List<string>>();
                for (int i = 0; i < partsOfPDNF.Count; i++)
                    Check(i, reducedParts, partsOfPDNF, singleParts);
                partsOfPDNF = reducedParts;
                if (reducedParts.Count == 0)
                    break;
            }
            List<List<string>> result = new List<List<string>>();
            for (int i = 0; i < partsOfPDNF.Count; i++)
            {
                if (!IsMatch(result, partsOfPDNF[i]))
                    result.Add(partsOfPDNF[i]);
            }
            for (int i = 0; i < singleParts.Count; i++)
                result.Add(singleParts[i]);
            return result;
        }

        static bool IsMatch(List<List<string>> result, List<string> partOfPDNF)
        {
            bool flag;
            if (result.Count == 0)
                return false;
            for(int i = 0; i < result.Count; i++)
            {
                flag = true;
                for(int j = 0; j < result[i].Count; j++)
                {
                    if (result[i][j] != partOfPDNF[j])
                        flag = false;
                }
                if (flag)
                    return true;
            }
            return false;
        }

        static bool Comparing(List<string> thisCombination, List<string> otherCombination)
        {
            int numOfInverseVars = 0;
            for (int i = 0; i < thisCombination.Count; i++)
            {
                if (thisCombination[i] == otherCombination[i]){}
                else if (!thisCombination[i].Contains(otherCombination[i]) && !otherCombination[i].Contains(thisCombination[i]))
                    return false;
                else
                {
                    numOfInverseVars++;
                    if (numOfInverseVars > 1)
                        return false;
                }
            }
            return true;
        }

        static void Check(int index, List<List<string>> result, List<List<string>> reduceParts, List<List<string>> singleParts)
        {
            bool hasNeighbours = false;
            for (int j = 0; j < reduceParts.Count; j++) 
            {
                if (Comparing(reduceParts[index], reduceParts[j]) && index != j) 
                {
                    List<string> reducedCombination = new List<string>();
                    foreach (var variable in reduceParts[index])
                    {
                        if (reduceParts[j].Contains(variable))
                            reducedCombination.Add(variable);
                        else
                            reducedCombination.Add("(-)");
                    }
                    if (CountingNumberOfVars(reducedCombination) == CountingNumberOfVars(reduceParts[index]) - 1)
                    {
                        if (!IsMatch(result, reducedCombination))
                            result.Add(reducedCombination);
                        hasNeighbours = true;
                    }
                }
            }
            if (!hasNeighbours && !IsMatch(singleParts, reduceParts[index]))
                singleParts.Add(reduceParts[index]);
        }

        static int CountingNumberOfVars(List<string> combination)
        {
            int count = 0;
            foreach(var variable in combination)
            {
                if (variable != "(-)")
                    count++;
            }
            return count;
        }

        public static void PrintRDNF(LogicalExpression logicExpression)
        {
            var RDNFParts = CalculatingPartsOfRDNF(logicExpression);
            List<string> strings = new List<string>();
            foreach (var RDNFPart in RDNFParts)
            {
                string part = "(";
                foreach (var variable in RDNFPart)
                {
                    if (variable[0] == '!')
                        part += "(" + variable + ")*";
                    else if(variable != "(-)")
                        part += variable + "*";                     
                }
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                strings.Add(part);
            }
            Console.WriteLine("\nReduced disjunction normal form:\n");
            Console.Write(strings[0]);
            for (int i = 1; i < strings.Count; i++)
                Console.Write("+" + strings[i]);
            Console.WriteLine();
        }

        static List<string> GetRDNF(LogicalExpression logicExpression)
        {
            var RDNFParts = CalculatingPartsOfRDNF(logicExpression);
            List<string> RDNF = new List<string>();
            foreach (var RDNFPart in RDNFParts)
            {
                string part = "(";
                foreach (var variable in RDNFPart)
                {
                    if (variable[0] == '!')
                        part += "(" + variable + ")*";
                    else if (variable != "(-)")
                        part += variable + "*";
                }
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                RDNF.Add(part);
            }
            return RDNF;
        }

        static List<List<bool>> GetQuineTable(LogicalExpression expression)
        {
            List<List<bool>> table = new List<List<bool>>();
            List<List<string>> reducedTerms = CalculatingPartsOfRDNF(expression);
            List<List<string>> fullTerms = TruthTableHandler.CalculatingPartsOfPDNF(expression);
            for(int i = 0; i < reducedTerms.Count; i++)
            {
                List<bool> tableRow = new List<bool>();
                for (int j = 0; j < fullTerms.Count; j++)
                    tableRow.Add(IsContain(reducedTerms[i], fullTerms[j]));
                table.Add(tableRow);
            }
            return table;
        }

        public static void PrintQuineTable(LogicalExpression expression)
        {
            List<List<bool>> table = GetQuineTable(expression);
            List<List<string>> reducedTerms = CalculatingPartsOfRDNF(expression);
            List<List<string>> fullTerms = TruthTableHandler.CalculatingPartsOfPDNF(expression);
            Console.Write("\t");
            for (int i = 0; i < fullTerms.Count; i++)
                Console.Write(fullTerms[i] + "\t");
            for(int i = 0; i < reducedTerms.Count; i++)
            {
                Console.Write(reducedTerms[i] + "\t");
                for (int j = 0; j < table[i].Count; j++)
                    Console.Write(table[i][j] ? "+\t" : "\t");
            }
        }

        static bool IsContain(List<string> reducedTerms, List<string> fullTerms)
        {
            for(int i = 0; i < fullTerms.Count; i++)
            {
                if (reducedTerms[i] != fullTerms[i] && reducedTerms[i] != "(-)")
                    return false;
            }
            return true;
        }

        static List<List<string>> GetCoreTerms(LogicalExpression expression)
        {
            List<List<string>> coreTerms = new List<List<string>>();
            List<List<bool>> table = GetQuineTable(expression);
            List<List<string>> reducedTerms = CalculatingPartsOfRDNF(expression);
            for (int i = 0; i < table[0].Count; i++)
            {
                int countOfTruth = 0, truthIndex =  0;
                for (int j = 0; j < table.Count; j++)
                    if (table[j][i])
                    {
                        countOfTruth++;
                        truthIndex = j;
                    }
                if (countOfTruth == 1 && !IsMatch(coreTerms, reducedTerms[truthIndex]))
                    coreTerms.Add(reducedTerms[truthIndex]);
            }
            return coreTerms;
        }

        static List<string> GetStringMDNF(LogicalExpression expression)
        {
            List<List<string>> coreTerms = GetCoreTerms(expression);
            List<List<string>> addictiveTerms = GetAddictiveTerms(expression);
            foreach(var term in addictiveTerms)
            {
                coreTerms.Add(term);
            }
            List<string> strTerms = new List<string>();
            for (int i = 0; i < coreTerms.Count; i++)
            {
                string strTerm = "(";
                foreach (var variable in coreTerms[i])
                {
                    if (variable[0] == '!')
                        strTerm += "(" + variable + ")*";
                    else if (variable != "(-)")
                        strTerm += variable + "*";
                }
                strTerm = strTerm.TrimEnd('*').Insert(strTerm.Length - 1, ")");
                strTerms.Add(strTerm);
            }
            return strTerms;
        }

        public static void PrintMDNF(LogicalExpression expression)
        {
            List<string> strTerms = GetStringMDNF(expression);
            List<string> necessaryImplicants = CheckingImplicants(expression);

            if (TruthTableHandler.IndexInterpretation(expression.ExpressionResult) == (int)Math.Pow(2, (int)(Math.Pow(2, expression.NumberOfVars))) - 1)
                Console.WriteLine("MDNF = 1 ");
            else if (TruthTableHandler.IndexInterpretation(expression.ExpressionResult) == 0)
                Console.WriteLine("Doesn't exist");
            else
            {
                string MDNF1 = strTerms[0];
                string MDNF2 = necessaryImplicants[0];
                for (int i = 1; i < necessaryImplicants.Count; i++)
                    MDNF2 += "+" + necessaryImplicants[i];
                for (int i = 1; i < strTerms.Count; i++)
                    MDNF1 += "+" + strTerms[i];
                Console.WriteLine("Minimized disjunctive normal form (Calculating-tabular method):\n" + MDNF1);
                Console.WriteLine("Minimized disjunctive normal form (Calculating method):\n" + MDNF2);
            }
        }

        static List<int> GetIndexesOfPureCols(LogicalExpression expression)
        {
            List<List<string>> coreTerms = GetCoreTerms(expression);
            List<List<string>> reduceTerms = CalculatingPartsOfRDNF(expression);
            List<List<bool>> table = GetQuineTable(expression);
            List<int> coverCols = new List<int>();
            List<int> pureCols = new List<int>();

            for (int i = 0; i < table.Count; i++)
            {
                if (IsInCoreTerms(coreTerms, reduceTerms[i]))
                    AddCoveredColsIndexes(table, coverCols, i);
            }
            for (int i = 0; i < table[0].Count; i++)
            {
                if (!coverCols.Contains(i))
                    pureCols.Add(i);
            }
            return pureCols;
        }

        static void AddCoveredColsIndexes(List<List<bool>> table, List<int> coverCols, int index)
        {
            for (int j = 0; j < table[index].Count; j++)
            {
                if (table[index][j])
                {
                    if (!coverCols.Contains(j))
                        coverCols.Add(j);
                }
            }
        }

        static List<int> GetCoveredColsIndexes(List<List<bool>> table, int index)
        {
            List<int> coveredCols = new List<int>();
            for (int j = 0; j < table[index].Count; j++)
            {
                if (table[index][j])
                {
                    if (!coveredCols.Contains(j))
                        coveredCols.Add(j);
                }
            }
            return coveredCols;
        }

        static bool IsInCoreTerms(List<List<string>> coreTerms, List<string> term)
        {
            foreach(var coreTerm in coreTerms)
            {
                for(int i = 0; i < coreTerm.Count; i++)
                {
                    if (term[i] != coreTerm[i])
                        break;
                    if (i == coreTerm.Count - 1)
                        return true;
                }
            }
            return false;
        }

        static List<List<string>> GetAddictiveTerms(LogicalExpression expression)
        {
            List<int> pureColsIndexes = GetIndexesOfPureCols(expression);
            List<List<string>> addictiveTerms = new List<List<string>>();
            if (pureColsIndexes.Count > 0)
            {
                List<List<bool>> table = GetQuineTable(expression);
                List<List<string>> reducedTerms = CalculatingPartsOfRDNF(expression);
                List<List<string>> coreTerms = GetCoreTerms(expression);
                Dictionary<int, List<int>> coveredColsIndexes = new Dictionary<int, List<int>>();
                int k = 1;
                List<int> indexes = new List<int>();
                bool isEveryColCovered = false;
                for (int i = 0; i < reducedTerms.Count; i++)
                {
                    if (!IsInCoreTerms(coreTerms, reducedTerms[i]))
                    {
                        coveredColsIndexes[i] = GetCoveredColsIndexes(table, i);
                        indexes.Add(i);
                    }
                }
                while (!isEveryColCovered)
                {
                    indexes.Sort();
                    List<List<int>> permutations = new List<List<int>>();
                    permutations.Add(PermutationsHandler.CopyOfRange(indexes, 0, k));
                    while (true)
                    {
                        List<int> perm = PermutationsHandler.GetNextKPermutation(k, indexes);
                        if (perm != null)
                            permutations.Add(perm);
                        else
                            break;
                    }
                    foreach (var permutation in permutations)
                    {
                        List<int> combinedCoveredColsIndexes = GetCombinedCoveredList(permutation, coveredColsIndexes);
                        if (IsEveryColCovered(pureColsIndexes, combinedCoveredColsIndexes))
                        {
                            isEveryColCovered = true;
                            foreach (var index in permutation)
                                addictiveTerms.Add(reducedTerms[index]);
                            break;
                        }
                    }
                    k++;
                }
            }
            return addictiveTerms;
        }

        static List<int> GetCombinedCoveredList(List<int> permutation, Dictionary<int, List<int>> coveredColsIndexes)
        {
            List<int> combinedCoveredList = new List<int>();
            foreach (var index in permutation)
            {
                for (int i = 0; i < coveredColsIndexes[index].Count; i++)
                    if (!combinedCoveredList.Contains(coveredColsIndexes[index][i]))
                        combinedCoveredList.Add(coveredColsIndexes[index][i]);
            }
            return combinedCoveredList;
        }

        static bool IsEveryColCovered(List<int> pureColsIndexes, List<int> combinedCoveredColsIndexes)
        {
            foreach(var index in pureColsIndexes)
            {
                if (!combinedCoveredColsIndexes.Contains(index))
                    return false;
            }
            return true;
        }
        
        public static List<string> CheckingImplicants(LogicalExpression expression)
        {
            List<string> necessaryImplicants = new List<string>();
            List<string> terms = GetRDNF(expression);
            string reducedForm = terms[0];
            for(int i = 1; i < terms.Count; i++)
            {
                reducedForm += "+" + terms[i];
            }
            LogicalExpression reducedFormExpression = new LogicalExpression(reducedForm);
            int reducedExpressionIndex = TruthTableHandler.IndexInterpretation(reducedFormExpression.ExpressionResult);
            if (terms.Count == 1)
                necessaryImplicants.Add(terms[0]);
            else
            {
                for (int i = 0; i < terms.Count; i++)
                {
                    string testForm = "";
                    for (int j = 0; j < terms.Count; j++)
                    {
                        if (i != j)
                            testForm += terms[j] + "+";
                    }
                    testForm = testForm.Remove(testForm.Length - 1, 1);
                    LogicalExpression testExpression = new LogicalExpression(testForm);
                    int testExpressionIndex = TruthTableHandler.IndexInterpretation(testExpression.ExpressionResult);
                    if (reducedExpressionIndex != testExpressionIndex)
                        necessaryImplicants.Add(terms[i]);
                }
            }
            return necessaryImplicants;
        }
    }
}
