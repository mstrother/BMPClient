using System;
using System.Threading;
using System.Threading.Tasks;
using BmpListener.Bmp;
using BmpListener.Serialization;
using System.IO;
using BmpListener.Bgp;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        static List<RouteMonitoring> routes = new List<RouteMonitoring>(1000000);

        static Program()
        {
            BmpMessageHandler.ProcessMessage = ProcessMessage;
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
            Task.Run(() => bmpListener.StartAsync(cts.Token));
            Console.WriteLine("Press any key to shutdown.");

            Console.ReadKey(true);

            cts.Cancel();
            bmpListener.CloseCompletion.Wait(TimeSpan.FromSeconds(20));
        }

        static async Task ProcessMessage(BmpMessage msg)
        {
            if (msg.BmpHeader.MessageType == BmpMessageType.RouteMonitoring)
            {
                if (((RouteMonitoring)msg).BgpUpdate.EndOfRib)
                {
                    var fileName = Path.GetRandomFileName();
                    var logFile = File.Create(fileName);
                    using (var sw = new StreamWriter(logFile))
                    {
                        foreach (var route in routes)
                        {
                            foreach (var nlri in route.BgpUpdate.Nlri)
                            {
                                var prefix = nlri.ToString();
                                var nextHop = route.BgpUpdate.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NextHop);
                                var asPath = route.BgpUpdate.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.AsPath);
                                var asnList = string.Join(" ", ((PathAttributeASPath)asPath).ASPaths[0].ASNs.Distinct().ToList());
                                sw.WriteLine($"{{\"Prefix\" : \"{prefix}\", \"NextHop\": \"{((PathAttributeNextHop)nextHop).NextHop.ToString()}\", \"AS Path\": \"{asnList}\"}},");
                            }
                        }
                    }
                    
                    Environment.Exit(0);
                }
                if ((((RouteMonitoring)msg).BgpUpdate.Nlri.Count > 0))
                {
                    routes.Add((RouteMonitoring)msg);
                }
            }
        }
    }
}

