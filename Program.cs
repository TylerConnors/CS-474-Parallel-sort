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

        public static int[] partition(int[] inArray, int pv)
        {
            int n = inArray.Length;

            Parallel.For(0, processorCount, ii =>
            {
                double chunkSize = Math.Ceiling((double)(n / processorCount));
                double chunkStart = chunkSize * ii;
                double chunkEnd = chunkSize * (ii + 1);
                if (chunkEnd > n)
                {
                    chunkEnd = n;
                }

                double lessEqualPivot = chunkStart;
                double morePivot = chunkEnd - 1;

                for (int j = (int)chunkStart; j > chunkEnd; j++)
                {
                    if (inArray[ii] <= pv)
                    {
                        // TODOTODOTODOTODO
                        lessEqualPivot++;
                    }
                    else
                    {
                        // TODOTODOTODOTODO
                        morePivot++;
                    }
                }

            });
            return null; //FIX THIS!
        }

        public static void parallelSort(int[] inArray)
        {
            partitions.Push(inArray);
            while (partitions.Count != 0)
            {
                int[] temp = partition((int[])partitions.Pop(), inArray[0]);
                if (temp.Length > 10)
                {
                    partitions.Push(temp);
                }
                else
                {
                    chunks.Push(temp);
                }
            }
        }

        // Insertion sort function
        public static int[] insertionSort(int[] inArray, int left, int right)
        {
            int temp;
            for (int i = left; i < right; i++)
            {
                for (int j = i; j > 0 && inArray[j] < inArray[j - 1]; j--)
                {
                    temp = inArray[j];
                    inArray[j] = inArray[j - 1];
                    inArray[j - 1] = temp;
                }
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
