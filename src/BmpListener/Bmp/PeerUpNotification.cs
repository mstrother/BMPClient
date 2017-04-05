using System;
using System.Net;
using BmpListener.Bgp;
using System.Linq;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : BmpMessage
    {
        public IPAddress LocalAddress { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public BgpOpenMessage SentOpenMessage { get; set; }
        public BgpOpenMessage ReceivedOpenMessage { get; set; }

        public override void Decode(ArraySegment<byte> data)
        {
            // For IPv4 peers, the most significant bit of PeerHeader.Flags will be set to 0.
            if (((PeerHeader.Flags & (1 << 7)) != 0))
            {
                var ipBytes = new byte[16];
                Array.Copy(data.Array, data.Offset, ipBytes, 0, 4);
                LocalAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data.Array, data.Offset + 12, ipBytes, 0, 4);
                LocalAddress = new IPAddress(ipBytes);
            }

            LocalPort = BigEndian.ToUInt16(data, 16);
            RemotePort = BigEndian.ToUInt16(data, 18);

            int offset = data.Offset + 20;
            int count = data.Count - 20;
            data = new ArraySegment<byte>(data.Array, offset, count);
            SentOpenMessage = BgpMessage.DecodeMessage(data) as BgpOpenMessage;

            offset += SentOpenMessage.Header.Length;
            count -= SentOpenMessage.Header.Length;
            data = new ArraySegment<byte>(data.Array, offset, count);
            ReceivedOpenMessage = BgpMessage.DecodeMessage(data) as BgpOpenMessage;
        }
    }
}