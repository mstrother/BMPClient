using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BmpListener.Bmp;

namespace BmpListener
{
    public class BmpListener
    {
        private readonly TcpListener tcpListener;

        public BmpListener(IPAddress ip, int port = 11019)
        {
            tcpListener = new TcpListener(ip, port);
            tcpListener.Start();
        }

        public BmpListener(int port = 11019)
        {
            tcpListener = new TcpListener(IPAddress.IPv6Any, port);
            tcpListener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            tcpListener.Start();
        }

        public async Task Start(Action<BmpMessage> action)
        {
            while (true)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                var task = Task.Run(() => ProcessClientAsync(tcpClient, action));
                await task;
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient, Action<BmpMessage> action)
        {
            using (var stream = tcpClient.GetStream())
            {
                while (true)
                {
                    var bmpHeaderBytes = new byte[6];
                    await stream.ReadAsync(bmpHeaderBytes, 0, 6); //add cancellation token
                    var header = new BmpHeader(bmpHeaderBytes);
                    var message = new BmpMessage(header);
                    if (header.Type != MessageType.Initiation)
                    {
                        var bmpPeerHeaderBytes = new byte[42];
                        await stream.ReadAsync(bmpPeerHeaderBytes, 0, 42); //add cancellation token
                        message.PeerHeader = new PeerHeader(bmpPeerHeaderBytes);
                        var bmpMsgBytes = new byte[header.Length - 48];
                        await stream.ReadAsync(bmpMsgBytes, 0, bmpMsgBytes.Length);
                        var data = new ArraySegment<byte>(bmpMsgBytes);
                        switch (header.Type)
                        {
                            case MessageType.RouteMonitoring:
                                message.Body = new RouteMonitoring(message, data);
                                break;
                            case MessageType.StatisticsReport:
                                message.Body = new StatisticsReport();
                                break;
                            case MessageType.PeerDown:
                                break;
                            case MessageType.PeerUp:
                                message.Body = new PeerUpNotification(message, data);
                                break;
                            case MessageType.Initiation:
                                message.Body = new BmpInitiation();
                                break;
                            case MessageType.Termination:
                                message.Body = new BmpTermination();
                                break;
                        }
                        action?.Invoke(message);
                    }
                }
            }
        }
    }
}
