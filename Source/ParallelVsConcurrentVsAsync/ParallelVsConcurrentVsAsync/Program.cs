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
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        string dataToWrite = string.Concat(Enumerable.Repeat("TestString", 5_000_000));
        Stopwatch sw;

        sw = Stopwatch.StartNew();

        // Warmup hard drive
        for (int i = 0; i < 30; i++)
        {
            WriteData(-i, dataToWrite);
        }
        for (int i = 0; i < 30; i++)
        {
            await WriteDataAsync(-30 - i, dataToWrite);
        }
        for (int i = 0; i < 30; i++)
        {
            WriteData(-60 - i, dataToWrite);
        }

        sw.Stop();
        Console.WriteLine($"Drive warmup took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// ------ Synchronous -------
        /// ---------------------------
        sw = Stopwatch.StartNew();

        WriteData(0, dataToWrite);
        WriteData(1, dataToWrite);
        WriteData(2, dataToWrite);
        WriteData(3, dataToWrite);

        sw.Stop();
        Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// ------ Asynchronous -------
        /// ---------------------------
        sw = Stopwatch.StartNew();

        //await WriteDataAsync(0, dataToWrite);
        //await WriteDataAsync(1, dataToWrite);
        //await WriteDataAsync(3, dataToWrite);
        //await WriteDataAsync(3, dataToWrite);
        var asyncTask0 = WriteDataAsync(0, dataToWrite);
        var asyncTask1 = WriteDataAsync(1, dataToWrite);
        var asyncTask2 = WriteDataAsync(2, dataToWrite);
        var asyncTask3 = WriteDataAsync(3, dataToWrite);

        await Task.WhenAll(asyncTask0, asyncTask1, asyncTask2, asyncTask3);
        sw.Stop();
        Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// ------ Synchronous --------
        /// ---------------------------
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

    static void WriteData(int id, string dataToWrite)
    {
        Console.WriteLine($"Starting {id}: {DateTime.UtcNow.TimeOfDay}");

        string docPath = Path.Combine(@"F:\SyncVsAsyncTest");
        using var outputFile = new StreamWriter(Path.Combine(docPath, $"SyncTest{id}.txt"));
        outputFile.Write(dataToWrite);

        Console.WriteLine($"Completed {id} - {DateTime.UtcNow.TimeOfDay}");
    }

    static async Task WriteDataAsync(int id, string dataToWrite)
    {
        Console.WriteLine($"Starting {id}: {DateTime.UtcNow.TimeOfDay}");

        string docPath = Path.Combine(@"F:\SyncVsAsyncTest");
        using var outputFile = new StreamWriter(Path.Combine(docPath, $"AsyncTest{id}.txt"));
        await outputFile.WriteAsync(dataToWrite);

        Console.WriteLine($"Completed {id} - {DateTime.UtcNow.TimeOfDay}");
    }
}