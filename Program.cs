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
        public static int SIZE;
        public static int[] randArray;
        public static int processorCount = Environment.ProcessorCount;
        public static Stack partitions = new Stack();
        public static Stack chunks = new Stack();
        public static int[] randArray2;
        public static int[] randArray3;


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
                inArray[i] = rand.Next(inArray.Length);
            }
            return inArray;
        }

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

            Console.WriteLine("nSmlEql[0]: " + nSmlEql[0]);
            Console.WriteLine("nSmlEql[1]: " + nSmlEql[1]);
            Console.WriteLine("nSmlEql[2]: " + nSmlEql[2]);
            Console.WriteLine("nSmlEql[3]: " + nSmlEql[3]);

            Console.WriteLine("nGrtTha[0]: " + nGrtTha[0]);
            Console.WriteLine("nGrtTha[0]: " + nGrtTha[1]);
            Console.WriteLine("nGrtTha[0]: " + nGrtTha[2]);
            Console.WriteLine("nGrtTha[0]: " + nGrtTha[3]);
            Console.WriteLine(" ");

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

            Console.WriteLine("nSmlEql[0]: " + nSmlEql[0]);
            Console.WriteLine("nSmlEql[1]: " + nSmlEql[1]);
            Console.WriteLine("nSmlEql[2]: " + nSmlEql[2]);
            Console.WriteLine("nSmlEql[3]: " + nSmlEql[3]);

            Console.WriteLine("nGrtTha[0]: " + nGrtTha[0]);
            Console.WriteLine("nGrtTha[1]: " + nGrtTha[1]);
            Console.WriteLine("nGrtTha[2]: " + nGrtTha[2]);
            Console.WriteLine("nGrtTha[3]: " + nGrtTha[3]);
            Console.WriteLine(" ");

            Parallel.For(0, processorCount, id =>
            {
                int count;
                int countb;

                if(id != 0)
                {
                    count = nSmlEql[id - 1];
                    countb = nGrtTha[id - 1] + 1;
                }
                else
                {
                    count = 0;
                    countb = 0;
                }

                Console.WriteLine("id: " + id + ". Count: " + count + ". Countb " + countb);

                double beginningOfSegment = widthOfSegment * id;
                double endOfSegment = widthOfSegment * (id + 1);

                for(int i = (int)beginningOfSegment; i < endOfSegment; i++)
                {
                    if (temp[i] <= pivot)
                    {
                        Console.WriteLine("id: " + id)
                        array[count] = temp[i];
                        count = count + 1;
                    }
                    else
                    {

                    }
                }
            });

            return 0;
        }

            Parallel.For(0, processorCount, id =>
            {
                double beginningOfSegment = widthOfSegment * id;
                double endOfSegment = widthOfSegment * (id + 1);

                int count;
                int countb;

                if (id != 0)
                {
                    count = nSmlEql[id - 1];
                    countb = nGrtTha[id - 1];
                }
                else
                {
                    count = 0;
                    countb = 0;
                }
                for (int i = (int)beginningOfSegment; i < endOfSegment; i++)
                {
                    if (temp[i] <= pivot)
                    {
                        array[count] = temp[i];
                        count++;
                    }
                    else
                    {
                        array[(int)endOfSegment - countb] = temp[i];
                        countb = countb++;
                    }
                }
            });
            return nSmlEql[processorCount - 1];
        }

        public static void doPSort(int[] inArray, int endSortIndex, int startSortIndex, int p)
        {
            int pivot = inArray[(startSortIndex + endSortIndex) / 2];

            if (endSortIndex - startSortIndex > 15)
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
            doPSort(inArray, inArray.Length, 0, processorCount);
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
            Console.WriteLine("Input the number of elements in the array");
            SIZE = Convert.ToInt32(Console.ReadLine());

            randArray = new int[SIZE];
            randArray2 = new int[SIZE];
            randArray3 = new int[SIZE];

            Console.WriteLine("Creating arrays");
            fillArray(randArray);
            Array.Copy(randArray, randArray2, SIZE);
            Array.Copy(randArray, randArray3, SIZE);
            Console.WriteLine("Finished creating arrays");

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Insertion sort start");
            stopwatch.Reset();
            stopwatch.Start();
            insertionSort(randArray, 0, randArray.Length);
            stopwatch.Stop();
            Console.WriteLine("Time of Insertion Sort: {0}", stopwatch.ElapsedMilliseconds);

            Console.WriteLine("Quicksort start");
            stopwatch.Reset();
            stopwatch.Start();
            quicksort(randArray2);
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


