using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksorts
{
    class Program
    {
        public static int SIZE = 100;
        public static int[] randArray = new int[SIZE];

        // Sequentially prints the contents of an array
        public static void printArray(int[] inArray)
        {
            for (int i = 0; i < inArray.Length; i++)
            {
                Console.WriteLine(inArray[i]);
            }
        }

        // Fill an array with random numbers
        public static int[] fillArray(int[] inArray)
        {
            Random rand = new Random();
            for (int i = 0; i < inArray.Length; i++)
            {
                inArray[i] = rand.Next(100);
            }

            return inArray;
        }

        // Entry function for the quicksort
        public static void quicksort(int[] inArray)
        {
            Quicksort(inArray, 0, (inArray.Length - 1));
        }

        // Recursive quicksort function, implementing 3-way partitioning
        private static void Quicksort(int[] inArray, int left, int right)
        {
            if (right <= left) return;

            int pivot = left;
            int leftIndex = left + 1;
            int rightIndex = right;
            int temp;

            int pivotValue = inArray[pivot];

            while (leftIndex <= rightIndex)
            {
                if (inArray[leftIndex] < pivotValue)
                {
                    temp = inArray[pivot];
                    inArray[pivot] = inArray[leftIndex];
                    inArray[leftIndex] = temp;
                    leftIndex++;
                    pivot++;
                }
                else if (inArray[leftIndex] > pivotValue)
                {
                    temp = inArray[leftIndex];
                    inArray[leftIndex] = inArray[rightIndex];
                    inArray[rightIndex] = temp;
                    rightIndex--;
                }
                else
                {
                    leftIndex++;
                }
            }

            Quicksort(inArray, left, pivot - 1);
            Quicksort(inArray, rightIndex + 1, right);
        }

        static void Main(string[] args)
        {
            fillArray(randArray);
            quicksort(randArray);
            printArray(randArray);
        }
    }
}
