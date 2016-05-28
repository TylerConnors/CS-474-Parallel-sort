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

        

        public static int partition(int[] inArray,int n, int pv)
        {
            int p = Environment.ProcessorCount;
            int[] temp = new int[n];
            Parallel.For(0, p, id =>
            {
                int chunkSize = (int)Math.Ceiling((double)(n / Environment.ProcessorCount));    //ws
                int chunkStart = chunkSize * id;                                                //b
                int chunkEnd = chunkSize * (id + 1);                                            //e

                int[] nSmEql = new int[n];
                int[] nGtTn = new int[n];

                if (chunkEnd > n)
                {
                    chunkEnd = n;
                }

                int l = chunkStart;                                                //l
                int g = chunkEnd - 1;                                                   //g

                for (int j = 0; j < Math.Log((double)p); j++)
                {
                if ((id - (2 * j)) >= 0)
            {
                nSmEql[id] = nSmEql[id - (2 * j)] + nSmEql[id];
                nGtTn[id] = nGtTn[id - (2 * j)] + nGtTn[id];
            }
                }


                for (int i = chunkStart; i < chunkEnd; i++)
                {
                    if (inArray[i] <= pv)
                    {
                        temp[l] = inArray[i];
                        l++;
                    }
                    else
                    {
                        temp[g] = inArray[i];
                        g--;
                    }
                }
                nSmEql[id] = l - chunkStart;
                nGtTn[id] = chunkEnd - g;

                int count, countb;

                if (id <> 0)  //don't know what this is
                {
                    count = nSmEql[id - 1];
                    countb = nGtTn[id - 1];
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

                return nSmEql[p - 1] ;

            });
             //FIX THIS!
        }

        public static void parallelSort(int[] inArray)
        {
            //don't know the values but it needs the correct ones, replace the 0's
            doPSort(inArray, 0, 0, 0);
        }

        public static void doPSort (int[] inArray,int e,int b, int p)
        {
            int pv = inArray[(b + e) / 2];
            int m = partition(inArray, (e - b), pv);  // the first part was shown as inArray[b] but than it would be an int, if there are errors look into this
            int pc = ((p * (m -b)) / (e-b));
            doPSort(inArray, b, m, pc);
            doPSort(inArray, m, e, p - pc);
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
            if (SIZE < 10000)
                populateArray(randArray);
            else
                fillArray(randArray);
            quicksort(randArray);
            printArray(randArray);
        }
    }
}

