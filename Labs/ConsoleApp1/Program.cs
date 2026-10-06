using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;

Console.WriteLine("Main method started " + DateTime.Now.TimeOfDay);
Thread.CurrentThread.Name = "Main";
// Running it on the main thread (Exercise 1a)
// DoNoisyWork();

// Running it in a separate thread (Exercise 1b)
Exercise1();

Thread.Sleep(1000);// This is to yield the main thread and allow other threads to run
Console.WriteLine("Main method completed " + DateTime.Now.TimeOfDay);

//Methods
void Exercise1()
{
    Thread t1 = new Thread(DoNoisyWork);
    Thread t2 = new Thread(DoNoisyWork);
    Thread t3 = new Thread(DoNoisyWork);
    
    t1.Name = "t1";
    t2.Name = "t2";
    t3.Name = "t3";

// Exercise 1(f) modifications:
    Thread.Sleep(5);
    t1.IsBackground = true;
    t2.IsBackground = true; 
    t3.IsBackground = true;

    t1.Start();
    t2.Start();
    t3.Start();

    t1.Join();
    t2.Join();
    t3.Join();

}

static void DoNoisyWork()
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    stopwatch.Start();

    string tname=Thread.CurrentThread.Name;
    int counter=0;
    int nCount=50; // you can vary the number of times the loop repeats
    for (int i=0; i<nCount; i++)
    {
        counter++;
        Console.WriteLine(tname+"count:"+counter);
        Thread.Sleep(1); // This is to yield the thread and allow other threads to run
    }
    stopwatch.Stop();
    Console.WriteLine(tname+"finished at "+DateTime.Now.TimeOfDay+" after RunTime "+ stopwatch.Elapsed);
}

void DoSilentWork()
{
    Stopwatch stopwatch=new Stopwatch();
    stopwatch.Start();

    string tname=Thread.CurrentThread.Name;
    int counter= 0;
    int nCount=100000000; // you can vary the number of times the loop repeats
    for (int i=0; i<nCount; i++)
    {
        counter++;
    }
    stopwatch.Stop();
    Console.WriteLine(tname+" has priority "+Thread.CurrentThread.Priority+" finished at "+DateTime.Now.TimeOfDay+" after RunTime "+stopwatch.Elapsed);

    
}

void Exercise2Sequential(int nTasks)
{ Stopwatch stopwatch=new Stopwatch();
    stopwatch.Start();
    
    for(int i=0; i<nTasks; i++)
    {
        DoSilentWork();
    }
    stopwatch.Stop();
    Console.WriteLine("Sequential execution completed after " + stopwatch.Elapsed);
}

void Exercise2Parallel(int nTasks)
{ Stopwatch stopwatch=new Stopwatch();
    stopwatch.Start();
    
    Thread[] threads = new Thread[nTasks];
    for(int i=0; i<nTasks; i++)
    {
        threads[i] = new Thread(DoSilentWork);
        threads[i].Name = "Thread" + i;
        threads[i].Start();
    }
    for(int i=0; i<nTasks; i++)
    {
        threads[i].Join();
    }
    stopwatch.Stop();
    Console.WriteLine("Parallel execution completed after " + stopwatch.Elapsed);
}

void Exercise3()

{ 
    Console.WriteLine("The value of the ThreadPriority Enum are:");
    foreach (ThreadPriority v in Enum.GetValues(typeof(ThreadPriority)))
        Console.WriteLine(v +" " + (int)v);

    Console.WriteLine("The priority of the Main Thread is:" + Thread.CurrentThread.Priority);

    int nTasks=4;
    Thread[] threads =new Thread[nTasks];
    for(int i=0; i<nTasks; i++)
    { 
        threads[i]=new Thread(DoSilentWork);
        threads[i].Name="t"+i;

        // (c) priority levels 1,2,3,4 and then repeating
        threads[i].Priority=(ThreadPriority)(i%5);
    }
    //(b) raise the thread 0, lower than 1
   //hreads[0].Priority=ThreadPriority.Highest;
   //hreads[1].Priority=ThreadPriority.Lowest;

    // control: the same change on the LAST two threads instead
   //hreads[nTasks-2].Priority=ThreadPriority.Highest;
   //hreads[nTasks-1].Priority=ThreadPriority.Lowest;
    for(int i=0; i<nTasks; i++)
    { threads[i].Start();
    }
}

void Exercise4()
{
    int nTasks=10;
    Thread[] threads =new Thread[nTasks];
    for(int i=0; i<nTasks; i++)
    { 
        threads[i]=new Thread(DoYieldingWork);
        threads[i].Name="t"+i;
    }

    for(int i=0; i<nTasks; i++)
    { threads[i].Start();
    }
    
}

void DoYieldingWork()
{
    Stopwatch stopwatch=new Stopwatch();
    stopwatch.Start();

    string tname = Thread.CurrentThread.Name;
    int counter=0;
    int nCount=100000000; // you can vary the number of times the loop repeats
    { for (int i =0; i<1000; i++) // inner loop to increase the work done
        { counter++;
        if(tname.Equals("t0")){ Thread.Yield(); }
        }
        stopwatch.Stop();
        Console.WriteLine(tname +" has priority "+Thread.CurrentThread.Priority+"task finished at" + DateTime.Now.TimeOfDay + " after RunTime "+stopwatch.Elapsed);
    }
}

void Exercise5()
{
    Clock clock =new Clock(100, true);
    Thread.Sleep(10000); 
}

public class Clock
{
    private int interval;
    Thread ticker;
    public Clock(int time, bool background)
    {
        interval = time;
        ticker = new Thread(Tick);
        ticker.Priority = ThreadPriority.Highest;
        ticker.IsBackground = background;
        ticker.Start();
    }

    void Tick()
    {
        while(true)
        {
            Console.WriteLine("Clock says: " + DateTime.Now.TimeOfDay);
            Thread.Sleep(interval);
        }
    }
}