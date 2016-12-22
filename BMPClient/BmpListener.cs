using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BmpListener.BMP;

namespace BmpListener
{
    public class BmpListener
    {
        private readonly TcpListener tcpListener;

        public BmpListener(IPAddress ip, int port = 11019)
        {
            tcpListener = new TcpListener(ip, port);
        }

        private async Task AcceptTcpClient(CancellationToken cts)
        {
            while (!cts.IsCancellationRequested)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                var task = ProcessClientAsync(tcpClient);
            }
        }

        private static async Task ProcessClientAsync(TcpClient tcpClient)
        {
            var clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
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
                            //WriteJson(message);
                        }
                    }
                }
        }
    }
}
