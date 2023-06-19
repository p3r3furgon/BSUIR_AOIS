using AOIS_7;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace AOIS_8
{

    class AssociativeMemory
    {
        private int tableSize;
        private List<List<int>> memoryTable;
        private List<List<int>> normalTable;

        public AssociativeMemory(int size)
        {
            tableSize = size;
            memoryTable = new List<List<int>>();
            normalTable = new List<List<int>>();
            for (int i = 0; i < 16; i++)
            {
                memoryTable.Add(new List<int>());
                for (int j = 0; j < 16; j++)
                {
                    memoryTable[i].Add(0);
                }
            }
        }

        private List<int> ShiftWord(int index, List<int> word)
        {
            int shift = word.Count - index;
            List<int> firstPart = word.GetRange(0, shift);
            List<int> secondPart = word.GetRange(shift, word.Count - shift);

            return secondPart.Concat(firstPart).ToList();
        }

        public void CreateEntry(List<int> word, int index)
        {
            List<int> shiftedWord = ShiftWord(index, word);
            for (int i = 0; i < memoryTable.Count; i++)
            {
                memoryTable[i][index] = shiftedWord[i];
            }
        }

        public List<List<int>> GenRandomWords(int size)
        {
            List<List<int>> words = new List<List<int>>();
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                List<int> word = new List<int>();
                string strWord = "";
                for (int j = 0; j < size; j++)
                {
                    int rand = random.Next(0, 2);
                    strWord += rand.ToString() + " ";
                    word.Add(rand);
                }
                words.Add(word);
                Console.WriteLine($"{i}. {strWord}");
                Thread.Sleep(20);
            } 
            return words;
        }

        public List<int> ReadWord(int index)
        {
            List<int> shiftedWord = new List<int>();
            for (int i = 0; i < memoryTable.Count; i++)
            {
                shiftedWord.Add(memoryTable[i][index]);
            }
            List<int> word = shiftedWord.GetRange(index, shiftedWord.Count - index)
                                        .Concat(shiftedWord.GetRange(0, index))
                                        .ToList();

            return word;
        }

        public List<List<int>> GetNormalTable()
        {
            List<List<int>> normalTable = new List<List<int>>();
            for (int i = 0; i < tableSize; i++)
            {
                normalTable.Add(ReadWord(i));
            }
            this.normalTable = normalTable;

            return normalTable;
        }

        public Dictionary<int, List<int>> Arithmetics(List<int> V)
        {
            Dictionary<int, List<int>> results = new Dictionary<int, List<int>>();
            List<List<int>> words = GetNormalTable();
            List<List<int>> suitableWords = words.FindAll(x => string.Join("", x.GetRange(0, 3)) == string.Join("", V));

            foreach (List<int> word in suitableWords)
            {
                List<int> part1 = word.GetRange(3, 4);
                List<int> part2 = word.GetRange(7, 4);
                results.Add(results.Count, word.GetRange(0, 11).Concat(Sum(part1, part2)).ToList());
            }

            return results;
        }

        private List<int> Sum(List<int> word1, List<int> word2)
        {
            List<int> el1 = new List<int>(word1);
            List<int> el2 = new List<int>(word2);
            List<int> result = new List<int>();
            int carry = 0;

            while (el1.Count > 0 && el2.Count > 0)
            {
                int digit1 = el1[el1.Count - 1];
                el1.RemoveAt(el1.Count - 1);
                int digit2 = el2[el2.Count - 1];
                el2.RemoveAt(el2.Count - 1);
                int res = digit1 ^ digit2 ^ carry;
                result.Insert(0, res);
                carry = (digit1 & digit2) | ((digit1 ^ digit2) & carry);
            }

            result.Insert(0, carry);
            return result;
        }

        public void ShowTable()
        {
            for (int i = 0; i < memoryTable.Count; i++)
            {
                Console.WriteLine(string.Join(" ", memoryTable[i]));
            }
        }

        public void ShowNormalTable()
        {
            List<List<int>> normalTable = GetNormalTable();
            for (int i = 0; i < normalTable.Count; i++)
            {
                Console.WriteLine(string.Join(" ", normalTable[i]));
            }
        }

        public List<int> ReadDigitColumn(int index)
        {
            return new List<int>(memoryTable[index]); 
        }

        public void WriteDigitColumn(int index, List<int> inputCol)
        {
            for(int i = 0; i < inputCol.Count; i++)
            {
                memoryTable[index][i] = inputCol[i];
            }
        }

        public List<int> F1(List<int> arg1, List<int> arg2)
        {
            return Conjunction(arg1, arg2);
        }

        public List<int> F14(List<int> arg1, List<int> arg2)
        {
            return Negation(Conjunction(arg1, arg2));
        }

        public List<int> F3(List<int> arg1)
        {
            return arg1;
        }

        public List<int> F12(List<int> arg1)
        {
            return Negation(arg1);
        }

        private List<int> Conjunction(List<int> word1, List<int> word2)
        {
            List<int> result = new List<int>();
            for(int i = 0; i < word1.Count; i++) 
            {
                if (word1[i] == word2[i] && word1[i] == 1)
                    result.Add(1);
                else 
                    result.Add(0);
            }
            return result;
        }

        private List<int> Negation(List<int> word)
        {
            List<int> result = new List<int>();
            for(int i = 0; i < word.Count; i++)
            {
                if (word[i] == 1)
                    result.Add(0);
                else
                    result.Add(1);
            }
            return result;
        }

        private dynamic ComparisonFlags(List<int> memoryWord, List<int> searchWord)
        {
            bool prevGFlag = false, prevLFlag = false;
            for (int i = 0; i < 16; i++)
            {
                bool memoryWordDigit = Convert.ToBoolean(memoryWord[i]);
                bool searchWordDigit = Convert.ToBoolean(searchWord[i]);
                bool nextGFlag = prevGFlag || (!searchWordDigit && memoryWordDigit && !prevLFlag);
                bool nextLFlag = prevLFlag || (searchWordDigit && !memoryWordDigit && !prevGFlag);
                prevGFlag = nextGFlag;
                prevLFlag = nextLFlag;
            }
            return new { GFlag = prevGFlag, LFlag = prevLFlag };
        }

        public int CompareWords(List<int> memoryWord, List<int> searchWord)
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

        public List<int> SearchTheNearestTopValue(List<int> inputWord)
        {
            List<List<int>> greaterValues = new List<List<int>>();
            normalTable = GetNormalTable();
            foreach (var currentWord in normalTable)
                if (CompareWords(currentWord, inputWord) == 1)
                    greaterValues.Add(currentWord);
            if (greaterValues.Count == 0)
                return null;
            List<int> result = greaterValues[0];
            for (int i = 1; i < greaterValues.Count; i++)
                if (CompareWords(result, greaterValues[i]) == 1)
                    result = greaterValues[i];
            return result;
        }

        public List<int> SearchTheNearestBottomValue(List<int> inputWord)
        {
            List<List<int>> lowerValues = new List<List<int>>();
            normalTable = GetNormalTable();
            foreach (var currentWord in normalTable)
                if (CompareWords(currentWord, inputWord) == -1)
                    lowerValues.Add(currentWord);
            if (lowerValues.Count == 0)
                return null;
            List<int> result = lowerValues[0];
            for (int i = 1; i < lowerValues.Count; i++)
                if (CompareWords(result, lowerValues[i]) == -1)
                    result = lowerValues[i];
            return result;
        }

    }
}