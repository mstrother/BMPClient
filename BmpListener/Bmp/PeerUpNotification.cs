using System;
using System.Linq;
using System.Net;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : IBMPBody
    {
        public PeerUpNotification(BmpMessage message, ArraySegment<byte> data)
        {
            ParseBody(message, data);
        }

        public IPAddress LocalAddress { get; set; }
        public ushort LocalPort { get; set; }
        public ushort RemotePort { get; set; }
        public BgpMessage SentOpenMessage { get; set; }
        public BgpMessage ReceivedOpenMessage { get; set; }

        public void ParseBody(BmpMessage message, ArraySegment<byte> data)
        {
            //if ((message.PeerHeader.Flags & (1 << 7)) != 0)
            //{
            LocalAddress = new IPAddress(data.Take(16).ToArray());
            //}

            LocalPort = data.ToUInt16(16);
            RemotePort = data.ToUInt16(18);

            data = new ArraySegment<byte>(data.Array, 20, data.Count - 20);

            SentOpenMessage = BgpMessage.GetBgpMessage(data);
        }
    }
}