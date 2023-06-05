using AOIS_3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var associativeProcessor = new AssociativeProcessor(10, 4);
            associativeProcessor.PrintTable();
            Console.WriteLine("Enter input word");
            string inputWord = Console.ReadLine();
            Console.WriteLine("Nearest top value: " + associativeProcessor.SearchTheNearestTopValue(inputWord));
            Console.WriteLine("Nearest bottom value: " + associativeProcessor.SearchTheNearestBottomValue(inputWord));
            var expression = new LogicalExpression("(!x*y)+(x*(!y))");
            TruthTableHandler.PrintTruthTable(expression.VarsPermutations, expression.UniqeVars, expression.ExpressionResult);
            associativeProcessor.BooleanFunctionSearch(expression);
        }
    }
}
