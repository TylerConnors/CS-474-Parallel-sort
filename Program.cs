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

        // Fill an array with random numbers, only used if the size is above 10,000
        public static int[] fillArray(int[] inArray)
        {
            Random rand = new Random();
            for (int i = 0; i < inArray.Length; i++)
            {
                inArray[i] = rand.Next();
            }

            return inArray;
        }


        public static int[] populateArray(int[] inArray)// creates an array of unique numbers from 0 to n-1
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

            //shuffles the list up
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
            if (SIZE < 10000)
                populateArray(randArray);
            else
                fillArray(randArray);
        }
    }
}

