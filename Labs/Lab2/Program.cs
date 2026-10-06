using System.Diagnostics;

Lab2Exercise1();

static void Lab2Exercise1()
{
    int NLOOPS = 100;
    Thread t1 =new Thread(DoSomeWork);
    t1.Name = "Thread 1";
    Thread t2 = new Thread(DoSomeWork);
    t2.Name = "Thread 2";
    DisplayStates("A", t1, t2);

    t1.Start(NLOOPS);
    DisplayStates("B", t1, t2);

    t2.Start(NLOOPS);
    DisplayStates("C", t1, t2);

    for (int i = 0; i < NLOOPS; i++)
    {
        Thread.Sleep(3);
        DisplayStates("C" + i, t1, t2);
    }
    t1.Join();
    t2.Join();
    DisplayStates("D", t1, t2);
}

static void DisplayStates(string label, Thread t1, Thread t2)
{
    Console.WriteLine("ThreadStates at " + label + ": "  
        + t1.Name + " is " + t1.ThreadState + ", "  
        + t2.Name + " is " + t2.ThreadState);
}
static void DoSomeWork(object? data)
{
    int NLOOPS = (int)data;
    object shared = new object();
    for(int i=0; i < NLOOPS; i++)
    {
       Thread.Sleep(2);
       IndividualWork(2);
       SharedWork(2, shared);
    }
}

static void IndividualWork(int ms) 
{ 
    DateTime finishTime = DateTime.Now.AddMilliseconds(ms); 
    while(DateTime.Now < finishTime) {/* use CPU until finsihTime*/} ; 
} 
 
static void SharedWork(int ms, object shared) 
{ 
    Monitor.Enter(shared); 
        DateTime finishTime = DateTime.Now.AddMilliseconds(ms); 
        while(DateTime.Now < finishTime) {/* use CPU until finsihTime*/} ; 
    Monitor.Exit(shared); 
}

Exercise2Manager(); 
 
static async void Exercise2Manager() 
{ 
    int NWORKERS = 100; 
    Console.WriteLine("This machine has "  
                + Environment.ProcessorCount + " processors"); 
    DateTime startTime = DateTime.Now; 
    Console.WriteLine("Manager starts at " + startTime) ; 
     
    //StartWorkerThreadsAndCoordinateWithJoin(NWORKERS); 
     //StartWorkerThreadsAndCoordinateWithAlive(NWORKERS); 
     StartWorkerTasksOnThreadPool(NWORKERS); 
 
    Console.WriteLine("All workers finished");  
    DateTime endTime = DateTime.Now; 
    Console.WriteLine("Exercise2Manager method ends at " + endTime + " after "  
            + (endTime-startTime) + " ms")  ; 
  } 
 
static void StartWorkerThreadsAndCoordinateWithJoin(int NWORKERS) 
{ 
    // store thread references so that we can check if they are done 
    List<Thread> threads = new List<Thread>();   
    for (int i = 0; i < NWORKERS; i++) { 
        Thread worker = new Thread(Work);       
        worker.Name = "worker-"+i;     // set the name of the thread       
        worker.Start();           // Start the thread      
        threads.Add(worker);           // Remember thread for later use 
    } 
    // will simply wait at the Join() method for other threads to complete 
    foreach (Thread thread in threads) { 
        thread.Join(); // manager waits for the thread to terminate 
    } 
} 
 
static void  Work() 
{ 
    Stopwatch stopwatch = new Stopwatch(); 
    stopwatch.Start(); 
 
    string tname = Thread.CurrentThread.Name; 
    Console.WriteLine(tname + " started at " + DateTime.Now); 
    int counter = 0;  
    int nCount = 100000000;      // you can vary the number of times the loop repeats   
    for (int i = 0; i < nCount; i++) { counter++;   } 
    stopwatch.Stop(); 
    Console.WriteLine(tname + " worker finished at " + DateTime.Now.TimeOfDay + 
                        " after RunTime " + stopwatch.Elapsed ); 
}

static void StartWorkerThreadsAndCoordinateWithAlive(int NWORKERS) 
{ 
    // store thread references so that we can check if they are done 
    List<Thread> threads = new List<Thread>(); 
    int runningNow; 
     
    for (int i = 0; i < NWORKERS; i++) { 
        Thread worker = new Thread(Work);       
        worker.Name = "worker-"+i;  // set the name of each thread       
        worker.Start();             // Start the thread      
        threads.Add(worker);        // Remember thread for later use 
    } 
 
    // uses .isAlive() to continually check if threads are running 
    int runningLastTime = NWORKERS; 
    do { 
        runningNow = 0;         
        foreach (Thread thread in threads) { 
           if (thread.IsAlive) runningNow++;        
        } 
        if(runningNow < runningLastTime){ 
            Console.WriteLine(runningNow + " threads running at " + DateTime.Now ); 
            runningLastTime = runningNow; 
        } 
    } while (runningNow > 0); 
} 

static void StartWorkerTasksOnThreadPool(int NWORKERS) 
{ 
List<Task> tasks = new List<Task>(); 
for (int n = 0; n < NWORKERS; n++) { 
ThreadPool.QueueUserWorkItem(WorkWrapper); 
} 
Console.WriteLine("Workers all started at: " + (DateTime.Now) + "ms"); 
Thread.Sleep(2000); 
} 
static void WorkWrapper(Object? state) 
{ 
Work(); 
}

await Exercise2ManagerAsync(); 
 
static async Task Exercise2ManagerAsync() 
{ 
    int NWORKERS = 100; 
 
    Console.WriteLine("This machine has "  
                + Environment.ProcessorCount + " processors"); 
    DateTime startTime = DateTime.Now; 
    Console.WriteLine("Manager starts at " + startTime) ; 
 
    await StartWorkersAsync(NWORKERS); 
 
    Console.WriteLine("All workers finished"); 
     
    DateTime endTime = DateTime.Now; 
    Console.WriteLine("Exercise2Manager method ends at " + endTime + " after "  
            + (endTime-startTime) + " ms")  ; 
  } 
 
static async Task StartWorkersAsync(int NWORKERS) 
{ 
    List<Task> tasks = new List<Task>(); 
                    
    for (int n = 0; n < NWORKERS; n++) { 
        await Task.Run(Work); 
    } 
    Console.WriteLine("Workers all added at: " + (DateTime.Now)); 
     
    await Task.WhenAll(tasks);  
} 