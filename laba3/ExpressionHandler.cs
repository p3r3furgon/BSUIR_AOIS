using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_3
{
    public class LogicExpression
    {
        public LogicExpression(string expression)
        {
            Expression = expression;
            AllVars = CountingAllVariablesInExpression(Expression);
            UniqeVars = CountingUniqeVariablesInExpression(AllVars);
            NumberOfVars = UniqeVars.Count;
            VarsValues = new Dictionary<string, bool>();
            Tokens = DividingExpressionOnTokens(Expression, AllVars);
        }

        public string Expression { get; set; }
        public List<string> UniqeVars { get; }
        public int NumberOfVars { get; }
        public Dictionary<string, bool> VarsValues { get; }
        public List<string> AllVars { get; }
        public List<string> Tokens { get; }
        public List<bool> ExpressionResult { get; }

        public static List<string> CountingAllVariablesInExpression(string expression)
        {
            List<string> allVars = new List<string>();
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsLetter(expression[i]))
                {
                    int varSize = 1;
                    while (i + varSize < expression.Length && char.IsDigit(expression[i + varSize]))
                        varSize++;
                    allVars.Add(expression.Substring(i, varSize));
                }
            }
            return allVars;
        }

        public static List<string> CountingUniqeVariablesInExpression(List<string> allVars)
        {
            List<string> uniqeVariables = new List<string>();
            foreach (var variable in allVars)
            {
                if (!uniqeVariables.Contains(variable))
                    uniqeVariables.Add(variable);
            }
            return uniqeVariables;
        }

        public static List<string> DividingExpressionOnTokens(string expression, List<string> allVars)
        {
            List<string> tokens = new List<string>();
            int varNumber = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsLetter(expression[i]))
                {
                    tokens.Add(allVars[varNumber]);
                    i += (allVars[varNumber].Length - 1);
                    varNumber++;
                }
                else if (expression[i] != ' ')
                {
                    if ((expression[i] == '-' && expression[i + 1] == '>') || (expression[i] == '=' && expression[i + 1] == '='))
                    {
                        tokens.Add(expression.Substring(i, 2));
                        i++;
                    }
                    else
                        tokens.Add(expression[i].ToString());
                }
            }
            return tokens;
        }

        static List<bool> CalculatingExpressionResults(int numberOfVars, Dictionary<string, bool> varsValues, List<string> uniqeVars , List<string> tokens, List<List<bool>> varsPermutation)
        {
            List<bool> expressionResults = new List<bool>();
            int numberOfPermutations = (int)Math.Pow(2, numberOfVars);
            for (int i = 0; i < numberOfPermutations; i++)
            {
                for (int j = 0; j < numberOfVars; j++)
                    varsValues[uniqeVars[j]] = varsPermutation[i][j];
                expressionResults.Add(LogicCalculator.Calculating(tokens, uniqeVars, varsValues));
            }
            return expressionResults;
        }
    }
    internal static class ExpressionHandler
    {
        
        static bool ComparingNumberOfBrackets(string expression)
        {
            try
            {
                int openParenthesisCount = (expression.Where(x => "(".IndexOf(x) != -1).Count());
                int closingParenthesisCount = (expression.Where(x => ")".IndexOf(x) != -1).Count());
                if (openParenthesisCount == closingParenthesisCount)
                    return true;
                else if (openParenthesisCount > closingParenthesisCount)
                    throw new Exception("Incorrect expression. Expected \')\'");
                else
                    throw new Exception("Incorrect expression. Unexpected \')\'");
                
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
        }

        static bool VarCheacking(string variable)
        {
            try
            {
                if (variable.Length > 1 && variable[1] == '0')
                    throw new Exception("Incorrect variable name \"" + variable + "\"");
                for (int i = 1; i < variable.Length; i++)
                    if (!char.IsDigit(variable[i]))
                        throw new Exception("Incorrect variable name \"" + variable + "\"");
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
        }

        static bool TokenCheacking(string token, List<string> uniqeVars)
        {
            string[] expressionSigns = { "!", "+", "*", "->", "==", ")", "(" };
            try
            {
                if (uniqeVars.Contains(token))
                    return VarCheacking(token);
                else if (expressionSigns.Contains(token))
                    return true;
                else
                    throw new Exception(" Unknown token \" " + token + "\"");
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
        }

        static bool TokensListChecking(List<string> tokens, List<string> uniqeVars)
        {
            try
            {
                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    if (uniqeVars.Contains(tokens[i]) && uniqeVars.Contains(tokens[i + 1]))
                        throw new Exception("Incorrect logic expression");
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static bool IsExpressionCorrect(string expression, List<string> tokens, List<string> uniqeVars)
        {
            if(uniqeVars.Count == 0)
            {
                Console.Error.WriteLine("There are no any variables in expression");
                return false;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (!TokenCheacking(tokens[i], uniqeVars))
                    return false;
            }
            return ComparingNumberOfBrackets(expression) && TokensListChecking(tokens, uniqeVars);
        }
    }
}