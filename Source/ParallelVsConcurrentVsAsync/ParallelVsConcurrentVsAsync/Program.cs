using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/**
 * This demo only works on Windows based devices
 */
public class Program
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    static async Task Main(string[] args)
    {
        Stopwatch sw;



        /// ---------------------------
        /// ------ Synchronous -------
        /// ---------------------------
        //sw = Stopwatch.StartNew();

        //SendRequest(0);
        //SendRequest(1);
        //SendRequest(2);
        //SendRequest(3);

        //sw.Stop();
        //Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// ------ Asynchronous -------
        /// ---------------------------
        // // Force app to only use 1 CPU Core
        // Process proc = Process.GetCurrentProcess();
        // long affinityMask = 0x0001;
        // proc.ProcessorAffinity = (IntPtr)affinityMask;
        // sw = Stopwatch.StartNew();

        // var asyncTask0 = SendRequestAsync(0);
        // var asyncTask1 = SendRequestAsync(1);
        // var asyncTask2 = SendRequestAsync(2);
        // var asyncTask3 = SendRequestAsync(3);
        // // DoWork(4); // This can be done while async tasks are running.

        // await Task.WhenAll(asyncTask0, asyncTask1, asyncTask2, asyncTask3);
        // sw.Stop();
        // Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// -------- Parallel ---------
        /// ---------------------------
        // sw = Stopwatch.StartNew();

        // var parallelTask0 = Task.Run(() => SendRequest(0));
        // var parallelTask1 = Task.Run(() => SendRequest(1));
        // var parallelTask2 = Task.Run(() => SendRequest(2));
        // var parallelTask3 = Task.Run(() => SendRequest(3));
        // DoWork(4);

        // await Task.WhenAll(parallelTask0, parallelTask1, parallelTask2, parallelTask3);
        // sw.Stop();
        // Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// --------------------------------------
        /// -------- Parallel/Concurrent ---------
        /// --------------------------------------
        sw = Stopwatch.StartNew();

        var parallelTasks = new List<Task>();
        for (int i = 0; i < 200; i++)
        {
            RunInParallel(i);
        }
        DoWork(201);

        await Task.WhenAll(parallelTasks);

        void RunInParallel(int id) => parallelTasks.Add(Task.Run(() => SendRequest(id)));
        sw.Stop();
        Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// --------------------------------------
        /// ----------- Asynchronous -------------
        /// --------------------------------------
        // sw = Stopwatch.StartNew();

        // var asynchronousTasks = new List<Task>();
        // for (int i = 0; i < 1000; i++)
        // {
        //     asynchronousTasks.Add(SendRequestAsync(i));
        // }
        // DoWork(1001);

        // await Task.WhenAll(asynchronousTasks);

        // sw.Stop();
        // Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ------------------------------------
        /// ------ Synchronous CPU-Bound--------
        /// ------------------------------------
        //sw = Stopwatch.StartNew();

        //DoWork(1);
        //DoWork(2);
        //DoWork(3);
        //DoWork(4);

        //sw.Stop();
        //Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// -------- Parallel ---------
        /// ---------------------------
        // sw = Stopwatch.StartNew();
        // var parallelTask0 = Task.Run(() => DoWork(1));
        // var parallelTask1 = Task.Run(() => DoWork(2));
        // var parallelTask2 = Task.Run(() => DoWork(3));
        // var parallelTask3 = Task.Run(() => DoWork(4));

        // await Task.WhenAll(parallelTask0, parallelTask1, parallelTask2, parallelTask3);
        // sw.Stop();
        // Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// ------- Concurrent --------
        /// ---------------------------
        // // Force app to only use 1 CPU Core
        // Process proc = Process.GetCurrentProcess();
        // long affinityMask = 0x0001;
        // proc.ProcessorAffinity = (IntPtr)affinityMask;

        // // Run the test
        // sw = Stopwatch.StartNew();

        // var concurrentTask0 = Task.Run(() => DoWork(1));
        // var concurrentTask1 = Task.Run(() => DoWork(2));
        // var concurrentTask2 = Task.Run(() => DoWork(3));
        // var concurrentTask3 = Task.Run(() => DoWork(4));

        // await Task.WhenAll(concurrentTask0, concurrentTask1, concurrentTask2, concurrentTask3);
        // sw.Stop();
        // Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");
    }

    static void DoWork(int id)
    {
        Console.WriteLine($"Starting {id}: {DateTime.UtcNow.TimeOfDay}");
        var sum = 0;
        for (var i = 1; i <= 10; i++)
            for (var j = 0; j <= 100_000_000; j++)
                sum += j / i;

        Console.WriteLine($"Completed {id} - {DateTime.UtcNow.TimeOfDay}");
    }

    static void SendRequest(int id)
    {
        Console.WriteLine($"Starting {id}: {DateTime.UtcNow.TimeOfDay}");

        var httpClient = new HttpClient();
        httpClient.Send(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5201/slow-response"));

        Console.WriteLine($"Completed {id} - {DateTime.UtcNow.TimeOfDay}");
    }

    static async Task SendRequestAsync(int id)
    {
        Console.WriteLine($"Starting {id}: {DateTime.UtcNow.TimeOfDay}");

        var httpClient = new HttpClient();
        await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5201/slow-response"));

        Console.WriteLine($"Completed {id} - {DateTime.UtcNow.TimeOfDay}");
    }
}