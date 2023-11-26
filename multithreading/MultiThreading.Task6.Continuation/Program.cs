/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code


            var parentTask = Task.Run(ParentTask);

            parentTask.ContinueWith(x => A_regardless(), TaskContinuationOptions.None);
            parentTask.ContinueWith(x => B_withoutSuccess(), TaskContinuationOptions.OnlyOnFaulted);
            parentTask.ContinueWith(x => C_sameThread(), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            parentTask.ContinueWith(x => D_OutsideThePool(), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.LongRunning);


            Console.ReadLine();
        }

        private static void ParentTask()
        {
            Console.WriteLine("Parent task is running");
            
            throw new Exception("Parent task failed");
        }

        private static void A_regardless()
        {
            Console.WriteLine("Task A (regardless) works!");

        }

        private static void B_withoutSuccess()
        {
            Console.WriteLine("Task B (without success) works!");
        }

        private static void C_sameThread()
        {
            Console.WriteLine("Task C works!");
        }

        private static void D_OutsideThePool()
        {
            Console.WriteLine("Task D works!");
        }


    }
}
