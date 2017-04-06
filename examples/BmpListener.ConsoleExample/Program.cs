using BmpListener.Bmp;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        IDisposable bmpSubscription;

        private static void Main()
        {
            // optimize IOCP performance
            //ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
            //ThreadPool.SetMinThreads(minWorkerThreads, Math.Max(16, minCompletionPortThreads));

            int threadCount = Environment.ProcessorCount;

            var cts = new CancellationTokenSource();
            var bmpListener = new BmpListener(11019);

            Console.WriteLine($"Starting new BMP agent on port {bmpListener.Port}.");
            Task.Run(() => bmpListener.StartAsync(cts.Token), cts.Token);
            Console.WriteLine($"BMP agent started at {DateTime.UtcNow:hh\\:mm\\:ss} (UTC).");
            Console.WriteLine();
            Console.WriteLine("Press any key to shutdown.");

            var logger = new ConsoleLogger();
            //Task.Run(() => logger.StartAsync(cts.Token), cts.Token);
            //bmpListener.OnMessageReceived.Subscribe(logger.LogMessageAsync);

            Console.ReadKey(true);
            cts.Cancel();
            bmpListener.CloseCompletion.Wait(TimeSpan.FromSeconds(20));
        }        
    }
}