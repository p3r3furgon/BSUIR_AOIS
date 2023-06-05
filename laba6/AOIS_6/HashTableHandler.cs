using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_6
{
    public class HashTable
    {
        public int hashTableSize;
        public HashNode[] hashTable;

        public HashTable(int tableSize)
        {
            this.hashTableSize = tableSize;
            this.hashTable = new HashNode[tableSize];
        }

        private int GetValueForKey(string word)
        {
            if (word.Length == 0)
            {
                return 0;
            }
            else if (word.Length <= 2)
            {
                return word[0];
            }
            else
            {
                Console.WriteLine(word + " " +  Enumerable.Range(0, word.Length)
                    .Select(i => (int)word[i] * (int)Math.Pow(2, i))
                    .Sum() % hashTableSize);
                return Enumerable.Range(0, word.Length)
                    .Select(i => (int)word[i] * (int)Math.Pow(2, i))
                    .Sum() % hashTableSize;
            }
        }

        public void SetNode(string hashKey, string data)
        {
            int tableIndex = GetValueForKey(hashKey);
            HashNode hashNode = hashTable[tableIndex];
            while (hashNode != null)
            {
                if (hashNode.HashKey == hashKey)
                {
                    hashNode.Data = data;
                    return;
                }
                Console.WriteLine($"Key {hashKey} collided with key {hashNode.HashKey}");
                hashNode = hashNode.Next;
            }
            HashNode newHashNode = new HashNode(hashKey, data);
            newHashNode.Next = hashTable[tableIndex];
            hashTable[tableIndex] = newHashNode;
        }

        public string GetNode(string hashKey)
        {
            int tableIndex = GetValueForKey(hashKey);
            HashNode hashNode = hashTable[tableIndex];
            while (hashNode != null)
            {
                if (hashNode.HashKey == hashKey)
                {
                    return hashNode.Data;
                }
                hashNode = hashNode.Next;
            }
            throw new KeyNotFoundException(hashKey);
        }

        public void PrintHashTable()
        {
            for(int i = 0; i < this.hashTable.Length; i++)
            {
                if (this.hashTable[i] is null)
                {
                    Console.WriteLine( i+1 + ". Null : None");
                }
                else
                {
                    Console.WriteLine( i+1 + ". " + this.hashTable[i].ToString());
                }
            }
        }

        public void DeleteNode(string hashKey)
        {
            int tableIndex = GetValueForKey(hashKey);
            HashNode hashNode = hashTable[tableIndex];
            HashNode prevHashNode = null;
            while (hashNode != null)
            {
                if (hashNode.HashKey == hashKey)
                {
                    if (prevHashNode != null)
                    {
                        prevHashNode.Next = hashNode.Next;
                    }
                    else
                    {
                        hashTable[tableIndex] = hashNode.Next;
                    }
                    return;
                }
                prevHashNode = hashNode;
                hashNode = hashNode.Next;
            }
            throw new KeyNotFoundException(hashKey);
        }
        

    }

    public class HashNode
    {
        public string HashKey { get; }
        public string Data { get; set; }
        public HashNode Next { get; set; }

        public HashNode(string hashKey, string data)
        {
            HashKey = hashKey;
            Data = data;
            Next = null;
        }

        public override string ToString()
        {
            return $"{HashKey}: {Data}";
        }
    }
}
