using AOIS_7;
using AOIS_3;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AOIS_8
{
    class Program
    {
        static void Main(string[] args)
        {
            AssociativeMemory AM = new AssociativeMemory(16);
            AM.ShowTable();
            Console.WriteLine("Write words");
            List<List<int>> words = AM.GenRandomWords(16);
            for (int i = 0; i < 16; i++)
            {
                AM.CreateEntry(words[i], i);
            }
            Console.WriteLine("Table:");
            AM.ShowTable();
            Console.WriteLine("Read words");
            for(int i = 0; i < 16; i++)
            {
                Console.WriteLine($"{i}. {string.Join(" ", AM.ReadWord(i))}");
            }
            List<int> inputWord = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Console.WriteLine("Write digital column");
            AM.WriteDigitColumn(1, inputWord);
            Console.WriteLine("Table:");
            AM.ShowTable();
            Console.WriteLine("Read words");
            for (int i = 0; i < 16; i++)
            {
                Console.WriteLine($"{i}. {string.Join(" ", AM.ReadWord(i))}");
            }
            Console.WriteLine("Logical functions");
            List<string> functions = new List<string>() { "F1", "F3", "F12", "F14" };
            Console.WriteLine(functions[0] + ": " + string.Join(" ", AM.F1(AM.GetNormalTable()[0], AM.GetNormalTable()[1])));
            Console.WriteLine(functions[1] + ": " + string.Join(" ", AM.F3(AM.GetNormalTable()[0])));
            Console.WriteLine(functions[2] + ": " + string.Join(" ", AM.F12(AM.GetNormalTable()[0])));
            Console.WriteLine(functions[3] + ": " + string.Join(" ", AM.F14(AM.GetNormalTable()[0], AM.GetNormalTable()[1])));
            Console.WriteLine("Sum");
            Dictionary<int, List<int>> results = AM.Arithmetics(new List<int>() { 1, 0, 1 });
            foreach (KeyValuePair<int, List<int>> result in results)
            {
                Console.WriteLine(string.Join(" ", result.Value));
            }

            Console.WriteLine("Nearest top value");
            Console.WriteLine(string.Join(" ", AM.SearchTheNearestTopValue(inputWord)));
            Console.WriteLine("Nearest bottom value");
            Console.WriteLine(string.Join(" ", AM.SearchTheNearestBottomValue(inputWord)));
            Console.ReadLine();
        }
    }
}