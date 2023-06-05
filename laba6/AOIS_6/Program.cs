using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HashTable hashTable = new HashTable(5);
            Console.WriteLine("Adding");
            hashTable.SetNode("Football", "Ronaldo");
            hashTable.SetNode("Basketball", "Curry");
            hashTable.SetNode("Hockey", "Ovechkin");
            hashTable.PrintHashTable();

            Console.WriteLine("Changing data");
            hashTable.SetNode("Football", "Messi");
            hashTable.PrintHashTable();

            Console.WriteLine("Deleting");
            hashTable.DeleteNode("Football");
            hashTable.PrintHashTable();

            Console.WriteLine("Deleting with not existing key");
            try
            {
                hashTable.DeleteNode("Chess");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            Console.WriteLine("Test 5 - collision test");
            hashTable.SetNode("Chess", "Carlssen");
            hashTable.SetNode("Football", "Neymar");
            hashTable.SetNode("Basketball", "James");
            hashTable.SetNode("Hockey", "Lisovsky");
            Console.WriteLine(hashTable.GetNode("Hockey"));
            hashTable.SetNode("Cybersport", "Kostylev");
            hashTable.PrintHashTable();

        }
    }
}
