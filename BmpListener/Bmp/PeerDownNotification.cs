using System;
using System.Linq;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerDownNotification : IBMPBody
    {
        public PeerDownNotification(ArraySegment<byte> data)
        {
            ParseBody(data);
        }

        public short Reason { get; private set; }
        public BgpMessage BGPNotification { get; set; }

        public void ParseBody(ArraySegment<byte> data)
        {
            Reason = data.First();
        }
    }
}