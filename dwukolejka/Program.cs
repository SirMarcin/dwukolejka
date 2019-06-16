using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dwukolejka
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Queue<Task> lp = new Queue<Task>();
            Queue<Task> hp = new Queue<Task>();
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Generate 25 random tasks...");

            for (int i = 0; i < 25; i++)
            {
                Task t = new Task(() =>
                {
                    int spn = rnd.Next(500, 5000);
                    Thread.Sleep(spn);
                });

                if (rnd.Next(0,2) == 1)
                {
                    hp.Enqueue(t);
                    Console.WriteLine($"Task #{t.Id} should be in hp");
                }
                else
                {
                    lp.Enqueue(t);
                    Console.WriteLine($"Task #{t.Id} should be in lp");
                }
            }

            Console.WriteLine("Assingn them to proper queue...");

            Console.WriteLine("High priority queue:");
            foreach (Task t in hp)
            {
                Console.WriteLine($"Task #{t.Id}"); 
            }

            Console.WriteLine("Low priority queue:");
            foreach (Task t in lp)
            {
                Console.WriteLine($"Task #{t.Id}");
            }

            Console.WriteLine("Running queues...");
            int hpLoop = hp.Count();

            Stopwatch swhp = new Stopwatch();
            swhp.Start();
                for (int i = 0; i < hpLoop; i++)
                {
                    var task = hp.Dequeue();
                    sw.Reset();
                    sw.Start();
                    task.Start();
                    task.Wait();
                    sw.Stop();
                    Console.WriteLine($"Task #{Task.CurrentId} processed in : {sw.ElapsedMilliseconds / 1000f}s");
                }

            int lpLoop = lp.Count();
            for (int i = 0; i < lpLoop; i++)
            {
                var task = lp.Dequeue();
                sw.Reset();
                sw.Start();
                task.Start();
                task.Wait();
                sw.Stop();
                Console.WriteLine($"Task #{Task.CurrentId} processed in : {sw.ElapsedMilliseconds / 1000f}s");
            }



            Console.ReadKey();
        }
    }
}
