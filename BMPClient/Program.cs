using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BmpListener.BMP;
using BmpListener.JSON;
using Newtonsoft.Json;

namespace BmpListener
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

            var cts = new CancellationToken();
            var listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            listener.Start();
            AcceptTcpClient(listener, cts).Wait(cts);
        }

        private static async Task AcceptTcpClient(TcpListener listener, CancellationToken cts)
        {
            while (!cts.IsCancellationRequested)
            {
                Console.WriteLine("Waiting for connection");
                var tcpClient = await listener.AcceptTcpClientAsync();
                var task = ProcessClientAsync(tcpClient);
            }
        }

        private static async Task ProcessClientAsync(TcpClient tcpClient)
        {
            var clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            Console.WriteLine($"Accepted a new connection from {clientEndPoint}");
            while (true)
                using (var stream = tcpClient.GetStream())
                {
                    while (true)
                    {
                        var bmpHeaderBytes = new byte[6];
                        var bmpPeerHeaderBytes = new byte[42];
                        await stream.ReadAsync(bmpHeaderBytes, 0, 6); //add cancellation token
                        var header = new Header(bmpHeaderBytes);
                        var message = new BMPMessage(header);
                        if (header.Type != BMPMessage.BMPMessageType.Initiation)
                        {
                            await stream.ReadAsync(bmpPeerHeaderBytes, 0, 42); //add cancellation token
                            message.PeerHeader = new PeerHeader(bmpPeerHeaderBytes);
                            var bmpMsgBytes = new byte[header.Length - 48];
                            await stream.ReadAsync(bmpMsgBytes, 0, bmpMsgBytes.Length);
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
                            WriteJson(message);
                        }
                    }
                }
        }

        private static void WriteJson(BMPMessage msg)
        {
            var json = JsonConvert.SerializeObject(msg);
            Console.WriteLine(json);
        }
    }
}