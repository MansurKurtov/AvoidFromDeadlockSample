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
            var thread1 = new Thread(() =>
            {
                Console.WriteLine("thread1 started");

                //lock (obj1)
                //{
                //    Thread.Sleep(100);
                //    lock (obj2)
                //    {
                //        Console.WriteLine("Task1 finished");
                //    }
                //}
                if (Monitor.TryEnter(obj1, 100))
                    try
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("thread1 succesfully finished");
                    }
                    finally
                    {
                        Monitor.Exit(obj1);
                    }
                else
                    Console.WriteLine("Thread1 waiting timeout!");
            });
            var thread2 = new Thread(() =>
            {
                Console.WriteLine("thread2 started");

                if (Monitor.TryEnter(obj1, 100))
                    try
                    {
                        Thread.Sleep(500);

                        Console.WriteLine("thread2 succesfully finished");
                    }
                    finally
                    {
                        Monitor.Exit(obj1);
                    }
                else
                    Console.WriteLine("Thread2 waiting timeout!");
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Console.WriteLine("All tasks finished");
            Console.ReadKey();
        }
    }
}
