using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerDownNotification : BmpMessage
    {
        public short Reason { get; private set; }
        public BgpMessage BGPNotification { get; set; }

        public override void Decode(byte[] data, int offset)
        {
            Reason = data[0];
        }
    }
}