using AOIS_3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOIS_7
{
    internal class AssociativeProcessor
    {
        private int _tableSize;
        private int _wordLength;
        private List<string> _table;

        public AssociativeProcessor(int tableSize, int wordLength)
        {
            _tableSize = tableSize;
            _wordLength = wordLength;
            _table = FillTable(tableSize, wordLength);
        }

        private List<string> FillTable(int tableSize, int wordLength)
        {
            List<string> table = new List<string>();
            for(int i = 0; i < tableSize; i++)
            {
                string word = "";
                for(int j = 0; j < wordLength; j++)
                {
                    var rand = new Random();
                    word += rand.Next(2).ToString();
                    Thread.Sleep(40);
                }
                table.Add(word);
            }
            return table;
        }

        private dynamic ComparisonFlags(string memoryWord, string searchWord)
        {
            bool prevGFlag = false, prevLFlag = false;
            for (int i = 0; i < _wordLength; i++)
            {
                int memorySign = int.Parse(memoryWord[i].ToString());
                int searchSign = int.Parse(searchWord[i].ToString());
                bool memoryWordDigit = Convert.ToBoolean(memorySign);
                bool searchWordDigit = Convert.ToBoolean(searchSign);
                bool nextGFlag = prevGFlag || (!searchWordDigit && memoryWordDigit && !prevLFlag);
                bool nextLFlag = prevLFlag || (searchWordDigit && !memoryWordDigit && !prevGFlag);
                prevGFlag = nextGFlag;
                prevLFlag = nextLFlag;
            }
            return new { GFlag = prevGFlag, LFlag = prevLFlag };
        }

        private int CompareWords(string memoryWord, string searchWord)
        {
            dynamic comparisonFlags = ComparisonFlags(memoryWord, searchWord);
            if (!comparisonFlags.GFlag && !comparisonFlags.LFlag)
                return 0;
            else if (comparisonFlags.GFlag && !comparisonFlags.LFlag)
                return 1;
            else if (!comparisonFlags.GFlag && comparisonFlags.LFlag)
                return -1;
            else
                throw new ArgumentException("Flags can't be equal to 1 at the same time");
        }

        public void PrintTable()
        {
            foreach (var word in _table)
                Console.WriteLine(word);
        }

        public string SearchTheNearestTopValue(string inputWord)
        {
            List<string> greaterValues = new List<string>();
            foreach(var currentWord in _table)
                if(CompareWords(currentWord, inputWord) == 1)
                    greaterValues.Add(currentWord);
            if (greaterValues.Count == 0)
                return "Nearest top value doesn't exist";
            string result = greaterValues[0];
            for(int i = 1; i < greaterValues.Count; i++)
                if (CompareWords(result, greaterValues[i]) == 1)
                    result = greaterValues[i];
            return result;
        }

        public string SearchTheNearestBottomValue(string inputWord)
        {
            List<string> lowerValues = new List<string>();
            foreach (var currentWord in _table)
                if (CompareWords(currentWord, inputWord) == -1)
                    lowerValues.Add(currentWord);
            if (lowerValues.Count == 0)
                return "Nearest bottom value doesn't exist";
            string result = lowerValues[0];
            for (int i = 1; i < lowerValues.Count; i++)
                if (CompareWords(result, lowerValues[i]) == -1)
                    result = lowerValues[i];
            return result;
        }

        public void BooleanFunctionSearch(LogicalExpression expression)
        {
            string tableValue = GetTableValue(expression);

            foreach(string word in _table)
            {
                if (CompareWords(word, tableValue) != 0)
                    continue;
                Console.WriteLine("Success, word " + word + " was found");
            }
            Console.WriteLine("word " + tableValue + " was not found");
        }

        private string GetTableValue(LogicalExpression formula)
        {
            var expressionResult = formula.ExpressionResult;
            string tableValue = "";
            foreach (var value in expressionResult)
            {
                tableValue += (value ? '1' : '0');
            }
            return tableValue;
        }
    }
}

