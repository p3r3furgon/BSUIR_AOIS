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

namespace AOIS_3
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
                int optionChoice;
                string expression = "";
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
                        expression = Console.ReadLine();
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
                        expression = testExpressions[expressionChoice - 1];
                        Console.Clear();
                        Console.WriteLine(expression + "\n");
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Bye!");
                        Environment.Exit(0);
                        break;
                }
                LogicalExpression taskExpression = new LogicalExpression(expression);
                int numberOfPermutations = (int)Math.Pow(2, taskExpression.NumberOfVars);
                List<List<bool>> varsPermutation = TruthTableHandler.Permutation(taskExpression.NumberOfVars);
                if (ExpressionHandler.IsExpressionCorrect(taskExpression.Expression, taskExpression.Tokens, taskExpression.UniqeVars))
                {
                    for (int i = 0; i < numberOfPermutations; i++)
                    {
                        for (int j = 0; j < taskExpression.NumberOfVars; j++)
                            varsValues[taskExpression.UniqeVars[j]] = varsPermutation[i][j];
                        expressionResult.Add(LogicCalculator.Calculating(taskExpression.Tokens, taskExpression.UniqeVars, varsValues));

                    }
                    TruthTableHandler.PrintTruthTable(varsPermutation, taskExpression.UniqeVars, expressionResult);
                    TruthTableHandler.PrintTotalResults(varsPermutation, taskExpression.UniqeVars, expressionResult, taskExpression);
                    FunctionMinimizationHandler.PrintMDNF(taskExpression);
                }
                Console.WriteLine("\n\nPress any key to continue . . .");
                Console.ReadLine();
            }
        }
    }
}