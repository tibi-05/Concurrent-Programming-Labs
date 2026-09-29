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