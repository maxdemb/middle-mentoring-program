/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {

        private static Semaphore s1 = new Semaphore(0, 1);
        private static Semaphore s2 = new Semaphore(0, 1);
        static void Main(string[] args)
        {

            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var sharedCollection = new List<int>();
            object collectionLock = new object();

            s1.Release();

            var addTask = Task.Run(() => {
                for (int i = 1; i <= 10; i++)
                {
                    s1.WaitOne();
                    Console.WriteLine("add: " + i);

                    sharedCollection.Add(i);

                    s2.Release();
                }
            });

            var printTask = Task.Run(() => {
                int i = 0;
                do
                {
                    s2.WaitOne();
                    //Console.WriteLine("print");

                    i = sharedCollection.Where(x => x != 0).Count();
                    var print = sharedCollection.GetRange(0, i);
                    Console.WriteLine($"{string.Join(", ", print)}");

                    s1.Release();
                } while (i < 10);
                
            });
       
            Console.ReadLine();
        }

    }
}
