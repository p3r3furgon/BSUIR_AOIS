using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task 1:\n");
            SBBS3.PrintTotalInformation();
            Console.WriteLine("Task 2:\n");
            D8421.PrintGlobalTruthTable();
            D8421.FindMinFormula();
            Console.ReadLine();
        }
    }
}
