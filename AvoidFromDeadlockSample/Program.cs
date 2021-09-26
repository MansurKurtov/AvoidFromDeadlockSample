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


        static void Main(string[] args)
        {
            object obj1 = new object();
            var task1 = new Task(() =>
            {
                Console.WriteLine("Task1 started");

                //lock (obj1)
                //{
                //    Thread.Sleep(100);
                //    lock (obj2)
                //    {
                //        Console.WriteLine("Task1 finished");
                //    }
                //}
                if (Monitor.TryEnter(obj1, 10))
                    try
                    {
                        Thread.Sleep(100);
                        Console.WriteLine("Task1 finished");
                    }
                    finally
                    {
                        Monitor.Exit(obj1);
                    }
            });
            var task2 = new Task(() =>
            {
                Console.WriteLine("Task2 started");

                if (Monitor.TryEnter(obj1, 100))
                    try
                    {
                        //Thread.Sleep(100);

                        Console.WriteLine("Task2 finished");
                    }
                    finally
                    {
                        Monitor.Exit(obj1);
                    }
            });

            Task.WaitAll(task1, task2);
            Console.WriteLine("All tasks finished");
        }
    }
}
