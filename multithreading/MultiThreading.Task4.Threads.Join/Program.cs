/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore semaphore = new Semaphore(0, 1);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            DecrementStateJoin(10);
            Console.WriteLine();
            DecrementStateSemaphore(10);

            Console.ReadLine();
        }

        static void DecrementStateJoin(int state)
        {
            if(state <= 0)
            {
                return;
            }

            var th = new Thread(() =>
            {
                state--;
                Console.Write(state + " ");
                DecrementStateJoin(state);
            });

            th.Start();
            th.Join();
        }

        static void DecrementStateSemaphore(object stateObject)
        {
            int state = (int)stateObject;
            if (state <= 0)
            {
                semaphore.Release();
                return;
            }

            state--;
            Console.Write(state + " ");
            ThreadPool.QueueUserWorkItem(DecrementStateSemaphore, state);
            semaphore.WaitOne();
        }
    }
}
