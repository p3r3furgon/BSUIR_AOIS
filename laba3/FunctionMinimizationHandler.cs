using AOIS_3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_3
{
    internal static class FunctionMinimizationHandler
    {
        public static List<List<string>> CreatingReducedDisjunctionForm(LogicExpression logicExpression)
        {
            List<List<string>> partsOfPDNF = TruthTableHandler.CalculatingPartsOfPDNF(logicExpression);
            List<List<string>> singleParts = new List<List<string>>();
            while (CountingNumberOfVars(partsOfPDNF[0]) > 2)
            {
                List<List<string>> reducedParts = new List<List<string>>();
                for (int i = 0; i < partsOfPDNF.Count - 1; i++)
                {
                    Check(i, reducedParts, partsOfPDNF, singleParts);
                }
                if (reducedParts.Count == 0)
                    break;
                else
                    partsOfPDNF = reducedParts;
            }
            List<List<string>> result = new List<List<string>>();
            for (int i = 0; i < partsOfPDNF.Count; i++)
            {
                if (!isContain(result, partsOfPDNF[i]))
                    result.Add(partsOfPDNF[i]);
            }
            for (int i = 0; i < singleParts.Count; i++)
            {
                result.Add(singleParts[i]);
            }
            return result;
        }

        //public static bool Check(List<string> thisCombination, List<string> otherCombination, List<List<string>> result)
        //{
        //    if (Comparing(thisCombination, otherCombination))
        //    {
        //        List<string> reducedCombination = new List<string>();
        //        foreach (var variable in thisCombination)
        //        {
        //            if (otherCombination.Contains(variable))
        //                reducedCombination.Add(variable);
        //        }
        //        if (reducedCombination.Count == thisCombination.Count - 1)
        //        {
        //            result.Add(reducedCombination);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool isContain(List<List<string>> result, List<string> partOfPDNF)
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

        //public static void Check(int index, List<List<string>> result, List<List<string>> reduceParts, List<List<string>> singleParts)
        //{
        //    bool hasNeighbours = false;
        //    for(int j = index+1; j < reduceParts.Count; j++) //need fix this(j = (0; redParts.Count), i!=j)
        //    {
        //        if (Comparing(reduceParts[index], reduceParts[j]))
        //        {
        //            List<string> reducedCombination = new List<string>();
        //            foreach (var variable in reduceParts[index])
        //            {
        //                if (reduceParts[j].Contains(variable))
        //                    reducedCombination.Add(variable);
        //                else
        //                    reducedCombination.Add("(-)");
        //            }
        //            if (CountingNumberOfVars(reducedCombination) == CountingNumberOfVars(reduceParts[index]) - 1)
        //            {
        //                result.Add(reducedCombination);
        //                hasNeighbours = true;
        //            }
        //        }
        //    }
        //    if (!hasNeighbours)
        //        singleParts.Add(reduceParts[index]);
        //}

        public static void Check(int index, List<List<string>> result, List<List<string>> reduceParts, List<List<string>> singleParts)
        {
            bool hasNeighbours = false;
            for (int j = 0; j < reduceParts.Count; j++) //need fix this(j = (0; redParts.Count), i!=j)
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
                        if (!isContain(result, reducedCombination))
                            result.Add(reducedCombination);
                        hasNeighbours = true;
                    }
                }
            }
            if (!hasNeighbours)
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
            var RDNFParts = CreatingReducedDisjunctionForm(logicExpression);
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


    }
}
