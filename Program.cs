using System;
using System.Collections;
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
        public static int processorCount = Environment.ProcessorCount;
        public static Stack partitions = new Stack();
        public static Stack chunks = new Stack();

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

        // creates an array of unique numbers from 0 to n-1
        public static int[] populateArray(int[] inArray)
        {
            int chunk = ((SIZE - 1) / Environment.ProcessorCount); //ensures best speed for computer
            Parallel.For(0, SIZE / chunk, j =>
            {
                int start = j * chunk;     // Inclusive start point for chunk
                int end = (j + 1) * chunk; // Exclusive endpoint for chunk
                for (int i = start; i < end; i++)
                {
                    inArray[i] = i;
                }
            });

            //shuffles the list
            Random rnd = new Random();
            int temp = 0;
            int temp2 = 0;
            for (int i = 0; i < SIZE - 1; i++)
            {
                temp = rnd.Next(SIZE - 1);
                if (i == temp)   //try to swap again if it picks the same index
                    temp = rnd.Next(SIZE - 1);
                temp2 = inArray[i];
                inArray[i] = inArray[temp];
                inArray[temp] = temp2;
            }
            return inArray;
        }

        public static int partition(int[] inArray, int n, int pv)
        {
            int[] temp = new int[n];
            int[] nSmEql = new int[n];
            int[] nGtTn = new int[n];

            Parallel.For(0, processorCount, processorID =>
            {
                int chunkSize = (int)Math.Ceiling((double)(n / processorCount));
                int chunkStart = chunkSize * processorID;
                int chunkEnd = chunkSize * (processorID + 1);

                if (chunkEnd > n)
                {
                    chunkEnd = n;
                }

                int lessThanPivotIndex = chunkStart;
                int greaterThanPivotIndex = chunkEnd - 1;

                for (int j = 0; j < Math.Ceiling(Math.Log((double)processorCount)); j++)
                {
                    if ((processorID - (Math.Pow(2, j))) >= 0)
                    {
                        nSmEql[processorID] = nSmEql[processorID - (2 * j)] + nSmEql[processorID];
                        nGtTn[processorID] = nGtTn[processorID - (2 * j)] + nGtTn[processorID];
                    }
                }


                for (int i = chunkStart; i < chunkEnd; i++)
                {
                    if (inArray[i] <= pv)
                    {
                        temp[lessThanPivotIndex] = inArray[i];
                        lessThanPivotIndex++;
                    }
                    else
                    {
                        temp[greaterThanPivotIndex] = inArray[i];
                        greaterThanPivotIndex--;
                    }
                }
                nSmEql[processorID] = lessThanPivotIndex - chunkStart;
                nGtTn[processorID] = chunkEnd - greaterThanPivotIndex;

                int count, countb;

                if (processorID != 0)
                {
                    count = nSmEql[processorID - 1];
                    countb = nGtTn[processorID - 1];
                }
                else
                {
                    count = 0;
                    countb = 0;
                }
                for (int i = chunkStart; i < chunkEnd; i++)
                {
                    if (temp[i] <= pv)
                    {
                        inArray[count] = temp[i];
                        count = count + 1;
                    }
                    else
                    {
                        inArray[chunkEnd - countb] = temp[i];
                        countb = countb - 1;
                    }
                }
            });

            return nSmEql[processorCount - 1];
        }

        public static void doPSort(int[] inArray, int endSortIndex, int startSortIndex, int p)
        {
            int pivot = inArray[(startSortIndex + endSortIndex) / 2];

            if (startSortIndex + endSortIndex < 15)
            {
                int m = partition(inArray, (endSortIndex - startSortIndex), pivot);  // the first part was shown as inArray[b] but than it would be an int, if there are errors look into this
                int pc = ((p * (m - startSortIndex)) / (endSortIndex - startSortIndex));
                doPSort(inArray, startSortIndex, m, pc);
                doPSort(inArray, m, endSortIndex, p - pc);
            }
            else
            {
                insertionSort(inArray, startSortIndex, endSortIndex);
            }
        }

        public static void parallelSort(int[] inArray)
        {
            //don't know the values but it needs the correct ones, replace the 0's
            doPSort(inArray, inArray.Length, 0, 0);
        }

        // Insertion sort function
        public static void insertionSort(int[] inArray, int left, int right)
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
            insertionSort(randArray, 0, randArray.Length);
            printArray(randArray);
        }
    }
}
