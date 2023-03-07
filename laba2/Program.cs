using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AOIS_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Dictionary<string, bool> varsValues = new Dictionary<string, bool>();
                List<bool> expressionResult = new List<bool>();
                Stack<string> stackSigns = new Stack<string>();
                Stack<bool> stackVars = new Stack<bool>();
                List<string> tokens = new List<string>();
                List<string> allVars = new List<string>();
                List<string> uniqeVars = new List<string>();
                int optionChoice;
                string logicExpression = "";
                Console.Clear();
                do
                {
                    Console.Clear();
                    Console.WriteLine("1. Enter your logic expression\n2. Check ready logic expressions\n3. Quit");
                    try
                    {
                        optionChoice = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        optionChoice = 0;
                    }

                } while (optionChoice <= 0 || optionChoice > 3);
                switch (optionChoice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Enter the logic expression:");
                        logicExpression = Console.ReadLine();
                        break;
                    case 2:
                        int expressionChoice = 0;
                        string[] testExpressions = TestExpressions.GetTestExpressions();
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Choose the number of expression\n");
                            for (int i = 0; i < testExpressions.Length; i++)
                                Console.WriteLine(i + 1 + ") " + testExpressions[i]);
                            Console.WriteLine();
                            try
                            {
                                expressionChoice = int.Parse(Console.ReadLine());
                            }
                            catch(Exception)
                            {
                                expressionChoice = 0;
                            }
                        } while (expressionChoice <= 0 || expressionChoice > testExpressions.Length);
                        logicExpression = testExpressions[expressionChoice - 1];
                        Console.Clear();
                        Console.WriteLine(logicExpression + "\n");
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Bye!");
                        Environment.Exit(0);
                        break;
                }
                ExpressionHandler.CountingVariablesInExpression(logicExpression, uniqeVars, allVars);
                ExpressionHandler.DividingExpressionOnTokens(logicExpression, tokens, allVars);
                int numberOfVariebles = uniqeVars.Count, numberOfPermutations = (int)Math.Pow(2, numberOfVariebles);
                var truthTable = TruthTableHandler.Permutation(numberOfVariebles);
                if (ExpressionHandler.IsExpressionCorrect(logicExpression, tokens, uniqeVars))
                {
                    for (int i = 0; i < numberOfPermutations; i++)
                    {
                        for (int j = 0; j < numberOfVariebles; j++)
                            varsValues[uniqeVars[j]] = truthTable[i][j];
                        expressionResult.Add(LogicCalculator.Calculating(tokens, uniqeVars, varsValues, stackSigns, stackVars));
                    }
                    TruthTableHandler.PrintTruthTable(truthTable, uniqeVars, expressionResult);
                    TruthTableHandler.PrintPDNF(truthTable, uniqeVars, expressionResult);
                    TruthTableHandler.PrintPCNF(truthTable, uniqeVars, expressionResult);
                }
                Console.WriteLine("\n\n\n\nPress any key to continue");
                Console.ReadLine();
            }
        }
    }
}
