using System;
using System.Linq;
using System.Net;
using BmpListener.Bgp;
using BmpListener.Extensions;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : BmpMessage
    {
        public PeerUpNotification(BmpHeader bmpHeader, ArraySegment<byte> data)
            : base(bmpHeader, data)
        {
            ParseBody(MessageData);
        }

        public IPAddress LocalAddress { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public BgpMessage SentOpenMessage { get; set; }
        public BgpMessage ReceivedOpenMessage { get; set; }

        public void ParseBody(ArraySegment<byte> data)
        {
            var offset = data.Offset;

            // For IPv4 peers, the most significant bit of PeerHeader.Flags will be set to 0.
            if (((PeerHeader.Flags & (1 << 7)) != 0))
            {
                var ipBytes = new byte[16];
                Array.Copy(data.Array, offset, ipBytes, 0, 16);
                LocalAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data.Array, offset + 12, ipBytes, 0, 4);
                LocalAddress = new IPAddress(ipBytes);
            }

            offset += 16;

            Array.Reverse(data.Array, offset, 2);
            LocalPort = BitConverter.ToUInt16(data.Array, offset);
            offset += 2;

            Array.Reverse(data.Array, offset, 2);
            RemotePort = BitConverter.ToUInt16(data.Array, offset);
            offset += 2;

            var count = data.Count - 20;
            data = new ArraySegment<byte>(data.Array, offset, count);
            SentOpenMessage = BgpMessage.GetBgpMessage(data);

            offset += SentOpenMessage.Header.Length;
            count -= SentOpenMessage.Header.Length;
            data = new ArraySegment<byte>(data.Array, offset, count);
            ReceivedOpenMessage = BgpMessage.GetBgpMessage(data);
        }
    }
}