using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_2
{
    internal static class ExpressionHandler
    {
        public static void CountingVariablesInExpression(string expression, List<string> vars, List<string> allVars)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsLetter(expression[i]))
                {
                    int varSize = 1;
                    while (i + varSize < expression.Length && char.IsDigit(expression[i + varSize]))
                        varSize++;
                    if (!vars.Contains(expression.Substring(i, varSize)))
                        vars.Add(expression.Substring(i, varSize));
                    allVars.Add(expression.Substring(i, varSize));
                }
            }
        }

        public static void DividingExpressionOnTokens(string expression, List<string> tokens, List<string> allVars)
        {
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
        }

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