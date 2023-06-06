using System.Diagnostics;

/**
 * This demo only works on Windows based devices
 */
public class Program
{
    private static string DataToWrite = string.Concat(Enumerable.Repeat("TestString", 70000000));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    static async Task Main(string[] args)
    {
        /// ---------------------------
        /// ------ Asynchronous -------
        /// ---------------------------
        var sw = Stopwatch.StartNew();

        var asyncTask0 = WriteData(0);
        var asyncTask1 = WriteData(1);
        var asyncTask2 = WriteData(2);
        var asyncTask3 = WriteData(3);

        await Task.WhenAll(asyncTask0, asyncTask1, asyncTask2, asyncTask3);
        sw.Stop();
        Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

        /// ---------------------------
        /// ------ Synchronous --------
        /// ---------------------------
        // sw = Stopwatch.StartNew();

        // DoWork(1);
        // DoWork(2);
        // DoWork(3);
        // DoWork(4);

        // sw.Stop();
        // Console.WriteLine($"Time took: {sw.ElapsedMilliseconds}ms, {sw.ElapsedTicks}ticks");

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

    static async Task WriteData(int id)
    {
        Console.WriteLine($"Starting {id}: {DateTime.UtcNow.TimeOfDay}");

        string docPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AsyncTest");
        using var outputFile = new StreamWriter(Path.Combine(docPath, $"Test{id}.txt"));
        await outputFile.WriteAsync(DataToWrite);

        Console.WriteLine($"Completed {id} - {DateTime.UtcNow.TimeOfDay}");
    }
}