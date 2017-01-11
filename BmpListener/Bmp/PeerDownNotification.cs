using System;
using System.Linq;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerDownNotification : IBMPBody
    {
        public PeerDownNotification(BmpMessage message, ArraySegment<byte> data)
        {
            ParseBody(message, data);
        }

        public short Reason { get; private set; }
        public BgpMessage BGPNotification { get; set; }

        public void ParseBody(BmpMessage message, ArraySegment<byte> data)
        {
            Reason = data.First();
        }
    }
}