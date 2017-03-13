using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BmpListener.Bmp;
using System.Diagnostics;
using BmpListener.Bgp;

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

        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public async Task Start()
        {
            while (true)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                var task = Task.Run(() => ProcessClientAsync(tcpClient));
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            using (var stream = tcpClient.GetStream())
            {
                while (true)
                {
                    var bmpHeaderBytes = new byte[Constants.BmpCommonHeaderLength];
                    await stream.ReadAsync(bmpHeaderBytes, 0, Constants.BmpCommonHeaderLength); //add cancellation token
                    var header = new BmpHeader(bmpHeaderBytes);
                    BmpMessage bmpMessage;
                    if (header.MessageType == BmpMessageType.Initiation)
                    {
                        bmpMessage = BmpMessage.Create(header);
                    }
                    else
                    {
                        var bmpMsgBytes = new byte[header.MessageLength - Constants.BmpCommonHeaderLength];
                        await stream.ReadAsync(bmpMsgBytes, 0, bmpMsgBytes.Length);
                        bmpMessage = BmpMessage.Create(header, bmpMsgBytes);
                        OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(bmpMessage));
                    }
                }
            }
        }
    }
}
