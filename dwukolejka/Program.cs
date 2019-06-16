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
                    sw.Reset();
                    sw.Start();
                    Thread.Sleep(spn);
                    sw.Stop();
                    Console.WriteLine($"Task #{Task.CurrentId} processed in : {sw.ElapsedMilliseconds/1000f}s");
          
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
            Console.WriteLine($"HP queue counts : {hp.Count()} of Tasks at the beggining.");
            Console.WriteLine($"LP queue counts : {lp.Count()} of Tasks at the beggining.");
            Console.WriteLine("Running queues...");
            Stopwatch swhp = new Stopwatch();
            Stopwatch swcp = new Stopwatch();
            swhp.Start();
            swcp.Start();
            int counter = 0;
            while (hp.Count() > 0)
                {
                    
                    if (swcp.ElapsedMilliseconds/1000f > 10 )
                    {
                    Console.WriteLine($"changing queue for Task#{hp.First().Id}");

                    //lp.Enqueue(hp.Dequeue());

                    var items = lp.ToArray();
                    lp.Clear();
                    lp.Enqueue(hp.Dequeue());
                    foreach (var item in items) lp.Enqueue(item);
                        
                    swcp.Restart();
                    }
                    else
                    {
                        var task = hp.Dequeue();
                        task.Start();
                        task.Wait();
                        counter++;
                    }
 
                }
            swhp.Stop();
            Console.WriteLine($"Number of processed tasks in hp: {counter}");
            Console.WriteLine($"Total time of procesing tasks of hp : {swhp.ElapsedMilliseconds/1000f}");
            Console.WriteLine($"Average time of procesing tasks of hp : {swhp.ElapsedMilliseconds/1000f/counter}");
            counter = 0;
            Stopwatch swlp = new Stopwatch();
            swlp.Start();
            while (lp.Count() > 0)
            {
                var task = lp.Dequeue();
                task.Start();
                task.Wait();
                counter++;
            }
            swlp.Stop();
            Console.WriteLine($"Number of processed tasks in lp: {counter}");
            Console.WriteLine($"Total time of procesing tasks of lp : {swlp.ElapsedMilliseconds / 1000f}");
            Console.WriteLine($"Average time of procesing tasks of lp : {swlp.ElapsedMilliseconds / 1000f / counter}");


            Console.ReadKey();
        }
    }
}
