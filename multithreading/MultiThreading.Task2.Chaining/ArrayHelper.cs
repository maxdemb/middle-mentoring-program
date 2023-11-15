using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    internal static class ArrayHelper
    {
        internal static int[] GenerateArrayOfRandomIntegers(int size)
        {
            var array = new int[size];
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(0, 10);
            }
            return array;
        }

        internal static int[] MultiplyArrayWithRandomInteger(int[] array)
        {
            var randomInteger = new Random().Next(0, 10);
            array = array.Select(x => x * randomInteger).ToArray();
            return array;
        }

        internal static void PrintArray(int[] array)
        {
            foreach (var item in array)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }

        internal static int[] SortArrayByAscending(int[] array) =>
            array.OrderBy(x => x).ToArray();

        internal static double CalculateTheAverageValue(int[] array) =>
            array.Average();
        
    }
}
