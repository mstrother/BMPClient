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
        private static readonly string semVer;

        static BmpListener()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;
            var gitVersionInformationType = assembly.GetType(assemblyName + ".GitVersionInformation");
            semVer = (string)gitVersionInformationType.GetField("SemVer").GetValue(null);
        }

        public static string Version { get { return semVer; } }

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
                    var bmpHeaderBytes = new byte[Constants.BmpCommonHeaderLength];
                    await stream.ReadAsync(bmpHeaderBytes, 0, Constants.BmpCommonHeaderLength); //add cancellation token
                    var header = new BmpHeader(bmpHeaderBytes);
                    BmpMessage bmpMessage;
                    if (header.MessageType == BmpMessage.Type.Initiation)
                    {
                        try
                        {
                            bmpMessage = BmpMessage.Create(header);
                        }
                        catch (NotSupportedException ex)
                        {
                            return;
                        }
                    }
                    else
                    {
                        var bmpMsgBytes = new byte[header.MessageLength - Constants.BmpCommonHeaderLength];
                        await stream.ReadAsync(bmpMsgBytes, 0, bmpMsgBytes.Length);
                        bmpMessage = BmpMessage.Create(header, bmpMsgBytes);
                    }
                    action?.Invoke(bmpMessage);
                }
            }
        }
    }
}
