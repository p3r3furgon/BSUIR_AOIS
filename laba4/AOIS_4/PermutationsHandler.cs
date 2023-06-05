using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_4
{
    internal static class PermutationsHandler
    {
        public static List<int> GetNextKPermutation(int k, List<int> number)
        {
            int n = number.Count;
            int i = k;
            for (; i < n && number[i] < number[k - 1];)
            {
                i = i + 1;
            }
            if (i < n)
            {
                swap(number, i, k - 1);
            }
            else
            {
                reverse(number, k - 1);
                int j = k - 2;
                for (; j >= 0 && number[j] > number[j + 1];)
                {
                    j = j - 1;
                }
                if (j < 0)
                {
                    return null;
                }
                else
                {
                    i = n - 1;
                    for (; i > j;)
                    {
                        if (number[i] > number[j])
                        {
                            break;
                        }
                        i = i - 1;
                    }
                    swap(number, i, j);
                    reverse(number, j);
                }
            }
            return CopyOfRange(number, 0, k);

        }

        static void reverse(List<int> permutation, int index)
        {
            int shift = index + 1;
            for (int i = 0; i < (permutation.Count - shift) / 2; i++)
            {
                int temp = permutation[shift + i];
                permutation[shift + i] = permutation[permutation.Count - i - 1];
                permutation[permutation.Count - i - 1] = temp;
            }
        }

        static void swap(List<int> permutation, int i, int j)
        {
            int temp = permutation[i];
            permutation[i] = permutation[j];
            permutation[j] = temp;
        }

        public static List<int> CopyOfRange(List<int> src, int start, int end)
        {
            int[] arrSrc = src.ToArray();
            int len = end - start;
            int[] dest = new int[len];
            Array.Copy(arrSrc, start, dest, 0, len);
            return dest.ToList<int>();
        }
    }
}
