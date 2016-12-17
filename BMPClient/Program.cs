using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BMPClient.BMP;
using BMPClient.JSON;
using Newtonsoft.Json;

namespace BMPClient
{
    internal class Program
    {
        private const string ipAddress = "0.0.0.0";
        private const int port = 11019;

        private static void Main()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new IPAddressConverter());
                settings.Converters.Add(new TestConverter());
                return settings;
            };

            var cts = new CancellationTokenSource();

            var listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            try
            {
                listener.Start();
                var task = AcceptTcpClient(listener, cts.Token);
                task.Wait(cts.Token);
            }
            finally
            {
                cts.Cancel();
                listener.Stop();
            }
        }

        private static async Task AcceptTcpClient(TcpListener listener, CancellationToken cts)
        {
            while (!cts.IsCancellationRequested)
            {
                Console.WriteLine("Waiting for connection");
                var tcpClient = await listener.AcceptTcpClientAsync();
                await EchoAsync(tcpClient, cts).ConfigureAwait(false);
            }
        }

        private static async Task EchoAsync(TcpClient client, CancellationToken ct)
        {
            var clientEndPoint = client.Client.RemoteEndPoint.ToString();
            Console.WriteLine($"Accepted a new connection from {clientEndPoint}");
            using (client)
            {
                using (var stream = client.GetStream())
                {
                    var bmpHeaderBytes = new byte[6];
                    var bmpPeerHeaderBytes = new byte[42];

                    while (!ct.IsCancellationRequested)
                    {
                        //var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15));
                        var headerReadTask = await stream.ReadAsync(bmpHeaderBytes, 0, 6, ct);
                        var header = new Header(bmpHeaderBytes);
                        var message = new BMPMessage(header);

                        //var completedTask = await Task.WhenAny(timeoutTask, amountReadTask).ConfigureAwait(false);
                        //var completedTask = await Task.WhenAny(headerReadTask).ConfigureAwait(false);
                        //if (completedTask == timeoutTask)                     
                        //var msg = Encoding.ASCII.GetBytes("Client timed out");
                        //await stream.WriteAsync(msg, 0, msg.Length);                                                                 

                        //check version

                        if (header.Type != BMPMessage.BMPMessageType.Initiation)
                        {
                            await stream.ReadAsync(bmpPeerHeaderBytes, 0, 42, ct);
                            message.PeerHeader = new PeerHeader(bmpPeerHeaderBytes);
                            var bmpMsgBytes = new byte[header.Length - 48];
                            await stream.ReadAsync(bmpMsgBytes, 0, bmpMsgBytes.Length, ct);

                            switch (header.Type)
                            {
                                case BMPMessage.BMPMessageType.RouteMonitoring:
                                    message.Body = new RouteMonitoring(message, bmpMsgBytes);
                                    break;
                                case BMPMessage.BMPMessageType.StatisticsReport:
                                    message.Body = new StatisticsReport();
                                    break;
                                case BMPMessage.BMPMessageType.PeerDown:
                                    break;
                                case BMPMessage.BMPMessageType.PeerUp:
                                    message.Body = new PeerUpNotification(message, bmpMsgBytes);
                                    break;
                                case BMPMessage.BMPMessageType.Initiation:
                                    message.Body = new BMPInitiation();
                                    break;
                                case BMPMessage.BMPMessageType.Termination:
                                    message.Body = new BMPTermination();
                                    break;
                            }
                        }
                        //await stream.WriteAsync(buf, 0, amountRead, ct).ConfigureAwait(false);
                        var json = JsonConvert.SerializeObject(message);
                        Console.WriteLine(json);
                    }
                }
            }
            Console.WriteLine("Client disconnected");
            Console.ReadKey();
        }
    }
}