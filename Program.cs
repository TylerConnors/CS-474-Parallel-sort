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

        // Find the median of three numbers
        public static int medianOfThree(int a, int b, int c)
        {
            if (a > b && a < c)
            {
                return a;
            }
            else if (a < b && b > c)
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        // Sort an array using quicksort
        public static int[] sortArray(int[] inArray, int n)
        {
            // Check to see if the array size is 1. If it is, end recursion.
            if (inArray.Length == 1)
            {
                return inArray;
            }

            Random pivotPicker = new Random();

            // Find a pivot
            int pivot = 0;

            int ran1, ran2, ran3;
            ran1 = inArray[pivotPicker.Next(n)];
            ran2 = inArray[pivotPicker.Next(n)];
            ran3 = inArray[pivotPicker.Next(n)];

            pivot = medianOfThree(ran1, ran2, ran3);

            // Partition

            // Recursion

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
