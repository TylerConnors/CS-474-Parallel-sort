using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;  // for the stop watch

namespace Quicksorts
{
    class Program
    {
        public static int[] randArray;
        public static int processorCount = Environment.ProcessorCount;

        // Partition an array with a pivot
        public static int partition(int[] array, double startIndex, double endIndex, int pivot)
        {
            int[] temp = new int[array.Length];
            int[] nSmlEql = new int[processorCount];
            int[] nGrtTha = new int[processorCount];
            double widthOfSegment = Math.Ceiling ((endIndex - startIndex) / processorCount);
            // Categorize the values in each segment
            Parallel.For(0, processorCount, id =>
            {
                double beginningOfSegment = widthOfSegment * id;
                double endOfSegment = widthOfSegment * (id + 1);
                if (endOfSegment > endIndex)
                {
                    endOfSegment = endIndex;
                }
                double lessThanPivot = beginningOfSegment;
                double greaterThanPivot = endOfSegment - 1;
                for (int i = (int)beginningOfSegment; i < (endOfSegment - 1); i++)
                {
                    if (array[i] <= pivot)
                    {
                        temp[(int)lessThanPivot] = array[i];
                        lessThanPivot++;
                    }
                    else
                    {
                        temp[(int)greaterThanPivot] = array[i];
                        greaterThanPivot--;
                    }
                }
                nSmlEql[id] = (int)(lessThanPivot - beginningOfSegment);
                nGrtTha[id] = (int)(endOfSegment - greaterThanPivot);
            });

            for (int id = 0; id < processorCount; id++)
            {
                for (int j = 0; j < (Math.Log(processorCount) - 1); j++)
                {
                    if (id - Math.Pow(2, j) >= 0)
                    {
                        nSmlEql[id] = nSmlEql[id - (int)(Math.Pow(2, j))] + nSmlEql[id];
                        nGrtTha[id] = nGrtTha[id - (int)(Math.Pow(2, j))] + nGrtTha[id];
                    }
                }
            }

            Parallel.For(0, processorCount, id =>
            {
                int count;
                int countb;

                if(id != 0)
                {
                    count = nSmlEql[id - 1];
                    countb = nGrtTha[id] - 1;
                }
                else
                {
                    count = 0;
                    countb = nGrtTha[id] - 1;
                }
                double beginningOfSegment = widthOfSegment * id;
                double endOfSegment = widthOfSegment * (id + 1);

                for(int i = (int)beginningOfSegment; i < (endOfSegment); i++)
                {
                    if (temp[i] <= pivot)
                    {
                        array[count] = temp[i];
                        count = count + 1;
                    }
                    else
                    {
                        array[((int)endIndex) - countb] = temp[i];
                        countb--;
                    }
                }
            });

            Console.WriteLine("Pivot: " + pivot);
            printArray(array);

            return nSmlEql[processorCount - 1];
        }

        public static void doPSort(int[] inArray, int startSortIndex, int endSortIndex, int p)
        {
            int pivot = inArray[(startSortIndex + endSortIndex) / 2];
            if (endSortIndex - startSortIndex > 8)
            {
                int m = partition(inArray, startSortIndex, endSortIndex, pivot);  // the first part was shown as inArray[b] but than it would be an int, if there are errors look into this
                int pc = ((p * (m - startSortIndex)) / (endSortIndex - startSortIndex));
                //doPSort(inArray, startSortIndex, m, pc);
                //doPSort(inArray, m, endSortIndex, p - pc);
            }
            //else
            //{
            //   insertionSort(inArray, startSortIndex, endSortIndex);
            //}
        }

        public static void parallelSort(int[] inArray)
        {
            doPSort(inArray, 0, inArray.Length, processorCount);
        }

        // Sequentially prints the contents of an array up to the first 500 
        public static void printArray(int[] inArray)
        {
            int temp = 500;

            if (inArray.Length < temp)
            {
                temp = inArray.Length;
            }

            for (int i = 0; i < temp; i++)
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

            int pivot = inArray[left];
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
            randArray = new int[40];
            fillArray(randArray);
            parallelSort(randArray);
        }
    }
}
