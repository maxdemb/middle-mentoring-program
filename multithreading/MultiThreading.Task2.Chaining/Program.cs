/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var task1 = Task.Run(() =>
            {
                var array = ArrayHelper.GenerateArrayOfRandomIntegers(10);
                ArrayHelper.PrintArray(array);
                return array;
            });
            var task2 = task1.ContinueWith(x => // Why X is needed?
            {
                var array = ArrayHelper.MultiplyArrayWithRandomInteger(task1.Result);
                ArrayHelper.PrintArray(array);
                return array;
            });
            var task3 = task2.ContinueWith(x =>
            {
                var array = ArrayHelper.SortArrayByAscending(task2.Result);
                ArrayHelper.PrintArray(array);
                return array;
            });
            var task4 = task2.ContinueWith(x => // we don't need to wait for task3 completition, since we use the same values. (order doesn't matter)
            {
                var averageValue = ArrayHelper.CalculateTheAverageValue(task2.Result);
                Console.WriteLine($"Average: {averageValue}");
            });       

            Console.ReadLine();
        }
    }
}
