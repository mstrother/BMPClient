using System;
using System.Threading;
using System.Threading.Tasks;
using BmpListener.Bmp;
using BmpListener.Serialization;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        static readonly IDisposable BmpMessageSubscription;
        
        static Program()
        {
            BmpMessageSubscription = BmpMessageHandler.MessageReceived.Subscribe(WriteJson);
        }

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
            //Console.WriteLine();
            Console.WriteLine("Press any key to shutdown.");
            
            Console.ReadKey(true);
            cts.Cancel();
            //bmpListener.CloseCompletion.Wait(TimeSpan.FromSeconds(20));
        }

        static void WriteJson(BmpMessage msg)
        {
            var json = msg.ToJson();
            Console.WriteLine(json);
        }
    }
}