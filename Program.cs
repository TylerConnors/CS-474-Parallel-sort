using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSort
{
    class Program
    {
        public static int SIZE = 100;
        public static int[] randArray = new int[SIZE];

        // Fill an array with random numbers
        public static int[] fillArray(int[] inArray)
        {
            Random rand = new Random();
            for (int i = 0; i < inArray.Length; i++)
            {
                inArray[i] = rand.Next();
            }

            return inArray;
        }

        // Sort an array
        public static int[] sortArray(int[] inArray)
        {
            return inArray;
        }

        // Sort an array using a parallel approach
        public static int[] sortArrayParallel(int[] inArray)
        {
            return inArray;
        }

        static void Main(string[] args)
        {
            fillArray(randArray);
        }
    }
}
