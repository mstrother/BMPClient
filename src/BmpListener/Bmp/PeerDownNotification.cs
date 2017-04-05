using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerDownNotification : BmpMessage
    {
        public short Reason { get; private set; }
        public BgpMessage BgpNotification { get; set; }

        public override void Decode(ArraySegment<byte> data)
        {
        }
    }
}