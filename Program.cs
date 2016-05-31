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
        public static int[] randArray2;
        public static int[] randArray3;

        public static int processorCount = Environment.ProcessorCount;

        // Partition an array with a pivot
        public static int partition(int[] array, double startIndex, double endIndex, int pivot)
        {
            int[] temp = new int[array.Length];
            int[] nSmlEql = new int[processorCount];
            int[] nGrtTha = new int[processorCount];
            double widthOfSegment = Math.Ceiling((endIndex - startIndex) / processorCount);
            // Categorize the values in each segment
            Parallel.For(0, processorCount, id =>
            {
                double beginningOfSegment = startIndex + widthOfSegment * id;
                double endOfSegment = startIndex + widthOfSegment * (id + 1);
                if (endOfSegment > endIndex)
                {
                    endOfSegment = endIndex;
                }
                double lessThanPivot = beginningOfSegment;
                double greaterThanPivot = endOfSegment - 1;
                for (int i = (int)beginningOfSegment; i < (endOfSegment); i++)
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
                nGrtTha[id] = (int)(endOfSegment - greaterThanPivot) - 1;
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

                if (id != 0)
                {
                    count = nSmlEql[id - 1];
                    countb = nGrtTha[id] - 1;
                }
                else
                {
                    count = 0;
                    countb = nGrtTha[id] - 1;
                }

                double beginningOfSegment = startIndex + widthOfSegment * id;
                double endOfSegment = startIndex + widthOfSegment * (id + 1);
                if (endOfSegment > endIndex)
                {
                    endOfSegment = endIndex;
                }

                for (int i = (int)beginningOfSegment; i < endOfSegment; i++)
                {
                    if (temp[i] <= pivot)
                    {
                        array[(int)startIndex + count] = temp[i];
                        count = count + 1;
                    }
                    else
                    {
                        array[((int)endIndex - 1) - countb] = temp[i];
                        countb--;
                    }
                }
            });
            return ((int)startIndex + nSmlEql[processorCount - 1]);
        }

        // Parallel quicksort with median-of-three pivot-picking to avoid endless recursion
        public static void doPSort(int[] inArray, int startSortIndex, int endSortIndex)
        {
            int temp = inArray.Length / 128;
            if (temp < 2)
                temp = 2;
            if (temp > 1000)
                temp = 1000;
            if (endSortIndex - startSortIndex > temp)
            {
                int pivot = medianOfThree(inArray[startSortIndex], inArray[endSortIndex - 1], inArray[(startSortIndex + endSortIndex) / 2]);
                int m = partition(inArray, startSortIndex, endSortIndex, pivot);
                doPSort(inArray, startSortIndex, m);
                doPSort(inArray, m, endSortIndex);
            }
            else
            {
                insertionSort(inArray, startSortIndex, endSortIndex);
            }
        }

        public static void parallelSort(int[] inArray)
        {
            doPSort(inArray, 0, inArray.Length);
        }

        // Median of three method
        public static int medianOfThree(int a, int b, int c)
        {
            if ((b < a && a < c) || (c < a && a < b))
            {
                return a;
            }
            else if ((a < b && b < c) || (c < b && b < a))
            {
                return b;
            }
            else
            {
                return c;
            }
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
                inArray[i] = rand.Next();
            }
            return inArray;
        }

        // Fill an array with sequential numbers 0 to n
        public static int[] sequentialArrayFill(int[] inArray)
        {
            for (int i = 0; i < inArray.Length; i++)
            {
                inArray[i] = i;
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


        // Shuffle function for an array, based on the Fisher-Yates shuffle
        public static int[] shuffleArray(int[] inArray)
        {
            Random rng = new Random();

            int n = inArray.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int temp = inArray[k];
                inArray[k] = inArray[n];
                inArray[n] = temp;
            }

            return inArray;
        }

        // basic quicksort found online
        private static void quickSort(int[] arr, int i, int j)
        {
            if (i < j)
            {
                int pos = partition(arr, i, j);
                quickSort(arr, i, pos - 1);
                quickSort(arr, pos + 1, j);
            }

        }

        private static int partition(int[] arr, int i, int j)
        {
            int pivot = arr[j];
            int small = i - 1;
            for (int k = i; k < j; k++)
            {
                if (arr[k] <= pivot)
                {
                    small++;
                    swap(arr, k, small);
                }
            }
            swap(arr, j, small + 1);
            return small + 1;
        }

        private static void swap(int[] arr, int k, int small)
        {
            int temp;
            temp = arr[k];
            arr[k] = arr[small];
            arr[small] = temp;
        }

        static void Main(string[] args)
        {
            //randArray = new int[200];
            //sequentialArrayFill(randArray);
            //shuffleArray(randArray);
            //parallelSort(randArray);
            //printArray(randArray);
            int SIZE = 0;
            Console.WriteLine("Input the number of elements in the array");
            SIZE = Convert.ToInt32(Console.ReadLine());

            randArray = new int[SIZE];
            randArray2 = new int[SIZE];
            randArray3 = new int[SIZE];

            Console.WriteLine("Creating arrays");
            sequentialArrayFill(randArray);
            shuffleArray(randArray);
            Array.Copy(randArray, randArray2, SIZE);
            Array.Copy(randArray, randArray3, SIZE);
            Console.WriteLine("Finished creating arrays");

            Stopwatch stopwatch = new Stopwatch();
            if (SIZE <= 10000)
            {
                Console.WriteLine("Insertion sort start");
                stopwatch.Reset();
                stopwatch.Start();
                insertionSort(randArray, 0, randArray.Length);
                stopwatch.Stop();
                Console.WriteLine("Time of Insertion Sort: {0}", stopwatch.ElapsedMilliseconds);
            }
            Console.WriteLine("Quicksort start");
            stopwatch.Reset();
            stopwatch.Start();
            quickSort(randArray2,0,SIZE-1);
            stopwatch.Stop();
            Console.WriteLine("Time of QuickSort: {0}", stopwatch.ElapsedMilliseconds);

            Console.WriteLine("Parallel sort start");
            stopwatch.Reset();
            stopwatch.Start();
            parallelSort(randArray3);
            stopwatch.Stop();
            Console.WriteLine("Time of Parallel Sort: {0}", stopwatch.ElapsedMilliseconds);


            Console.WriteLine("Press any key to print parallel array.");
            Console.ReadKey();
            printArray(randArray3);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

    }
}
}

