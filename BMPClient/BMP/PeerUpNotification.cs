using System;
using System.Net;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : IBMPBody
    {
        public PeerUpNotification(BmpMessage message, byte[] data)
        {
            ParseBody(message, data);
        }

        public IPAddress LocalAddress { get; set; }
        public ushort LocalPort { get; set; }
        public ushort RemotePort { get; set; }
        public BgpMessage SentOpenMessage { get; set; }
        public BgpMessage ReceivedOpenMessage { get; set; }

        public void ParseBody(BmpMessage message, byte[] data)
        {
            if ((message.PeerHeader.Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Buffer.BlockCopy(data, 0, ipBytes, 0, 16);
                LocalAddress = new IPAddress(ipBytes);
            }

            LocalPort = data.ToUInt16(16);
            RemotePort = data.ToUInt16(18);

            //var dataSegment = new ArraySegment<byte>(data, 20, data.Length - 20);

            //SentOpenMessage = BgpMessage.GetBgpMessage(dataSegment);
        }
    }
}