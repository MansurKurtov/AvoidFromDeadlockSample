using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvoidFromDeadlockSample
{
    class Program
    {
        static object obj1 = new object();
        static object obj2 = new object();

        static void Main(string[] args)
        {
            var task1 = new Task(() =>
            {
                Console.WriteLine("Task1 started");

                lock (obj1)
                {
                    Thread.Sleep(100);
                    lock (obj2)
                    {
                        Console.WriteLine("Task1 finished");
                    }
                }
            });
            var task2 = new Task(() =>
            {
                Console.WriteLine("Task2 started");

                lock (obj2)
                {
                    Thread.Sleep(100);
                    lock (obj1)
                    {
                        Console.WriteLine("Task2 finished");
                    }
                }
            });
            Task.WaitAll(task1, task2);
            Console.WriteLine("All tasks finished");
        }
    }
}
