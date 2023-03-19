using AOIS_3;
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
        public static List<List<string>> CalculatingPartsOfRDNF(LogicExpression logicExpression)
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

        public static bool IsMatch(List<List<string>> result, List<string> partOfPDNF)
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

        public static bool Comparing(List<string> thisCombination, List<string> otherCombination)
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

        public static void Check(int index, List<List<string>> result, List<List<string>> reduceParts, List<List<string>> singleParts)
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

        public static int CountingNumberOfVars(List<string> combination)
        {
            int count = 0;
            foreach(var variable in combination)
            {
                if (variable != "(-)")
                    count++;
            }
            return count;
        }

        public static void PrintRDNF(LogicExpression logicExpression)
        {
            var RDNFParts = CalculatingPartsOfRDNF(logicExpression);
            List<string> strings = new List<string>();
            foreach (var RDNFPart in RDNFParts)
            {
                string part = "";
                foreach (var variable in RDNFPart)
                {
                    if (variable[0] == '!')
                        part += "(" + variable + ")*";
                    else if(variable != "(-)")
                        part += variable + "*";                     
                }
                part = part.Remove(part.Length - 1, 1);
                strings.Add(part);
            }
            Console.WriteLine("\nReduced disjunction normal form:\n");
            Console.Write(strings[0]);
            for (int i = 1; i < strings.Count; i++)
                Console.Write("+" + strings[i]);
            Console.WriteLine();
        }

        public static List<List<bool>> BuildingQuineTable(LogicExpression expression)
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

        public static bool IsContain(List<string> reducedTerms, List<string> fullTerms)
        {
            for(int i = 0; i < fullTerms.Count; i++)
            {
                if (reducedTerms[i] != fullTerms[i] && reducedTerms[i] != "(-)")
                    return false;
            }
            return true;
        }

        public static List<int> GetUselessIndexes(LogicExpression expression)
        {
            List<int> indexesOfUselessTerms = new List<int>();
            List<List<bool>> table = BuildingQuineTable(expression);
            for(int i = 0; i < table.Count; i++)
            {
                List<bool> activeRow = new List<bool>(table[i]);
                for(int j = 0; j < table[i].Count; j++)
                    table[i][j] = false;
                if (!IsTermNecessary(table))
                    indexesOfUselessTerms.Add(i);
                table[i] = activeRow;
            }
            return indexesOfUselessTerms;
        }

        public static List<List<string>> GetCoreTerms(LogicExpression expression)
        {
            List<List<string>> coreTerms = new List<List<string>>();
            List<List<bool>> table = BuildingQuineTable(expression);
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
                    coreTerms.Add(new List<string>(reducedTerms[truthIndex]));
            }
            return coreTerms;
        }

    public static bool IsTermNecessary(List<List<bool>> table)
        {
            for(int i = 0; i < table[0].Count; i++)
            {
                List<bool> rowCondition = new List<bool>();
                for (int j = 0; j < table.Count; j++)
                    rowCondition.Add(table[j][i]);
                if (!rowCondition.Contains(true))
                    return true;
            }
            return false;
        }

        public static List<string> GetStringDDNF(LogicExpression expression)
        {

            List<List<string>> terms = GetCoreTerms(expression);
            List<string> strTerms = new List<string>();
            for (int i = 0; i < terms.Count; i++)
            {
                string strTerm = "(";
                foreach (var variable in terms[i])
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

        public static void PrintDDNF(LogicExpression expression)
        {
            List<string> strTerms = GetStringDDNF(expression);
            if(strTerms.Count > 0)
            { 
                string DDNF = strTerms[0];
                for (int i = 1; i < strTerms.Count; i++)
                    DDNF += "+" + strTerms[i];
                Console.WriteLine("Dead-end disjunctive normal form:\n" + DDNF);
            }
        }
    }
}
