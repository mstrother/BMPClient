using System;
using System.Threading;
using System.Threading.Tasks;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        static int port = 11019;

        private static void Main()
        {
            // optimize IOCP performance
            //ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
            //ThreadPool.SetMinThreads(minWorkerThreads, Math.Max(16, minCompletionPortThreads));

            int threadCount = Environment.ProcessorCount;

            var cts = new CancellationTokenSource();
            var bmpListener = new BmpStation(port);

            Console.WriteLine($"Starting new BMP agent on port {port}.");
            Task.Run(() => bmpListener.StartAsync(cts.Token), cts.Token);
            Console.WriteLine($"BMP agent started at {DateTime.UtcNow.ToString("hh\\:mm\\:ss")} (UTC).");
            Console.WriteLine();
            Console.WriteLine("Press any key to shutdown.");

            var logger = new ConsoleLogger();
            bmpListener.OnMessageReceived += logger.LogMessageAsync;
            Task.Run(() => logger.StartAsync(cts.Token), cts.Token);

            Console.ReadKey(true);
            cts.Cancel();
            bmpListener.CloseCompletion.Wait(TimeSpan.FromSeconds(20));
        }
    }
}