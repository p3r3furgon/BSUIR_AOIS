using AOIS_3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_3
{
    internal class KarnaughMapHandler
    {
        public class Cell
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Cell(double x, double y)
            {
                X = x;
                Y = y;
            }

            public bool Equals(Cell other)
            {
                if (other == null)
                    return false;
                return X == other.X && Y == other.Y;
            }

            public bool GreaterThan(Cell other)
            {
                if (other == null)
                    return false;
                return Distance(new Cell(0, 0)) > other.Distance(new Cell(0, 0));
            }

            public bool LessThan(Cell other)
            {
                if (other == null)
                    return false;
                return Distance(new Cell(0, 0)) < other.Distance(new Cell(0, 0));
            }

            public bool GreaterThanOrEqual(Cell other)
            {
                if (other == null)
                    return false;
                return Distance(new Cell(0, 0)) >= other.Distance(new Cell(0, 0));
            }

            public bool LessThanOrEqual(Cell other)
            {
                if (other == null)
                    return false;
                return Distance(new Cell(0, 0)) <= other.Distance(new Cell(0, 0));
            }

            public double Distance(Cell other)
            {
                if (other == null)
                    throw new ArgumentNullException("Other object must be Cell type");
                return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
            }

            public override string ToString()
            {
                return $"Cell: ({X}, {Y})";
            }
        }

        public static class KarnaughMapSolver
        {
            private static List<List<Cell>> MakeKarnaughGroups(LogicalExpression expression, bool isPDNF)
            {
                List<Cell> AllNeedNumberCells = new List<Cell>();
                var karnaughtMap = KarnaughMapBuilder.GetKarnaughMap(expression);
                int needNumber = isPDNF ? 1 : 0;
                for (int i = 0; i < karnaughtMap.Count; i++)
                {
                    for (int j = 0; j < karnaughtMap[i].Count; j++)
                    {
                        if (karnaughtMap[i][j] == needNumber)
                        {
                            AllNeedNumberCells.Add(new Cell(i, j));
                        }
                    }
                }

                List<List<Cell>> AllPossibleGroups = new List<List<Cell>>();
                int max_possible_group_size = (int)Math.Log(karnaughtMap.SelectMany(row => row).Count(), 2);
                for (int i = 0; i <= max_possible_group_size; i++)
                {
                    AllPossibleGroups.AddRange(Combinations(AllNeedNumberCells, (int)Math.Pow(2, i)));
                }

                AllPossibleGroups = AllPossibleGroups.Where(group => IsGroupRectangular(group)).ToList();
                AllPossibleGroups = AllPossibleGroups.Where(group => IsKarnaughNeigbourhood(group)).ToList();

                return AllPossibleGroups;
            }

            private static bool IsKarnaughNeigbourhood(List<Cell> group)
            {
                if (group.Count <= 1)
                    return true;

                double massCenterX = group.Average(cell => cell.X);
                double massCenterY = group.Average(cell => cell.Y);
                Cell massCenterCell = new Cell(massCenterX, massCenterY);
                var distances = group.Select(cell => (Cell: cell, Distance: massCenterCell.Distance(cell))).ToList();
                var minDisctance = distances[0];

                foreach (var distance in distances)
                    if (distance.Distance < minDisctance.Distance)
                        minDisctance = distance;

                if (minDisctance.Cell.X == massCenterCell.X || minDisctance.Cell.Y == massCenterCell.Y)
                    return Math.Abs(minDisctance.Distance - Math.Floor(minDisctance.Distance)) == 0.5;


                double projectionX = Math.Abs(massCenterX - minDisctance.Cell.X);
                double projectionY = Math.Abs(massCenterY - minDisctance.Cell.Y);

                return Math.Abs(projectionX - Math.Floor(projectionX)) == 0.5 && Math.Abs(projectionY - Math.Floor(projectionY)) == 0.5;
            }

            private static bool IsGroupRectangular(List<Cell> group)
            {
                if (group.Count <= 1)
                    return true;

                if (group.Select(cell => cell.X).Distinct().Count() == 1 || group.Select(cell => cell.Y).Distinct().Count() == 1)
                    return true;

                return CheckForCommonRectangularity(group);
            }

            private static bool CheckForCommonRectangularity(List<Cell> group)
            {
                int xLinesAmount = group.Select(cell => cell.X).Distinct().Count();
                int yLinesAmount = group.Select(cell => cell.Y).Distinct().Count();

                foreach (var cell in group)
                {
                    var xySymmetry = new[] { 0, 0 };

                    foreach (var otherCell in group)
                    {
                        if (cell == otherCell)
                            continue;

                        if (cell.X == otherCell.X)
                            xySymmetry[0]++;
                        else if (cell.Y == otherCell.Y)
                            xySymmetry[1]++;
                    }

                    if (!xySymmetry.SequenceEqual(new[] { xLinesAmount - 1, yLinesAmount - 1 }))
                    {
                        return false;
                    }
                }

                double massCenterX = group.Average(cell => cell.X);
                double massCenterY = group.Average(cell => cell.Y);
                Cell massCenter = new Cell(massCenterX, massCenterY);
                var distances = group.Select(cell => massCenter.Distance(cell)).ToList();

                return distances.Distinct().Count() == distances.Count / 2 || distances.Distinct().Count() == 1;
            }

            private static List<List<T>> Combinations<T>(List<T> elements, int k)
            {
                List<List<T>> result = new List<List<T>>();
                CombinationsUtil(elements, k, 0, new List<T>(), result);
                return result;
            }

            private static void CombinationsUtil<T>(List<T> elements, int k, int start, List<T> combination, List<List<T>> result)
            {
                if (k == 0)
                {
                    result.Add(new List<T>(combination));
                    return;
                }

                for (int i = start; i <= elements.Count - k; i++)
                {
                    combination.Add(elements[i]);
                    CombinationsUtil(elements, k - 1, i + 1, combination, result);
                    combination.RemoveAt(combination.Count - 1);
                }
            }

            private static List<List<Cell>> GetObligatoryGroups(List<List<Cell>> karnaughGroups)
            {
                karnaughGroups = karnaughGroups.OrderByDescending(group => group.Count).ToList();
                List<Cell> coveredCells = new List<Cell>();
                List<List<Cell>> obligatoryGroups = new List<List<Cell>>();

                foreach (var group in karnaughGroups)
                {
                    if (group.Any(cell => !coveredCells.Contains(cell)))
                    {
                        obligatoryGroups.Add(group);
                        coveredCells.AddRange(group);
                    }
                }
                return obligatoryGroups;
            }

            public static string GetResult(List<List<Cell>> obligatoryGroups, LogicalExpression expression, bool isPDNF)
            {
                List<string> terms = new List<string>();
                string outbracketsOperator = isPDNF? "+": "*";
                foreach(var group in obligatoryGroups)
                {
                    terms.Add(CreateTermByGroup(group, expression, isPDNF));
                }
                return string.Join(outbracketsOperator, terms);
            }

            private static string CreateTermByGroup(List<Cell> group, LogicalExpression expression, bool isPDNF)
            {
                var needNumber = isPDNF ? 1 : 0;
                var insideOperator = isPDNF ? "*" : "+";
                var term = new StringBuilder();
                var variables = expression.UniqeVars;
                var grayCode = KarnaughMapBuilder.GetGrayCode(variables.Count);
                var karnaughMap = KarnaughMapBuilder.GetKarnaughMap(expression);
                int a = grayCode[0].Count / 2;
                int b = grayCode[0].Count - a;
                var yVars = variables.Take(a).ToList();
                var xVars = variables.GetRange(a, variables.Count - a);
                var yVector = KarnaughMapBuilder.GetGrayCode(a);
                yVector.Reverse();
                var xVector = KarnaughMapBuilder.GetGrayCode(b);
                Dictionary<string, List<int>> variablesValues = new Dictionary<string, List<int>>();

                foreach (var variable in variables)
                {
                    variablesValues[variable] = new List<int>();
                }
                foreach (var cell in group)
                {
                    for (int i = 0; i < xVars.Count; i++)
                    {
                        variablesValues[xVars[i]].Add(int.Parse(xVector[(int)cell.Y][i].ToString()));
                    }
                    for (int i = 0; i < yVars.Count; i++)
                    {
                        variablesValues[yVars[i]].Add(int.Parse(yVector[(int)cell.X][i].ToString()));
                    }
                }
                List<string> literals = new List<string>();
                foreach (var variable in variables)
                {
                    if (variablesValues[variable].Distinct().Count() <= 1)
                    {
                        string literal = "";
                        if (needNumber != variablesValues[variable][0])
                            literal += "!";
                        literals.Add(literal + variable); 
                    }
                }

                return "(" + string.Join(insideOperator, literals) + ")";
            }

            public static void MinimizeWithKarnaughMap(LogicalExpression expression, bool isPDNF)
            {
                KarnaughMapBuilder.PrintKarnaughMap(expression, isPDNF);
                var obligatoryGroups = GetObligatoryGroups(MakeKarnaughGroups(expression, isPDNF));
                Console.WriteLine(GetResult(obligatoryGroups, expression, isPDNF));
            }
        }
    }
}
