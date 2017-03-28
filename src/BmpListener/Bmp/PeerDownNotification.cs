using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerDownNotification : BmpMessage
    {
        public PeerDownNotification(byte[] data)
            : base(data)
        {
            Decode(data);
        }

        public short Reason { get; private set; }
        public BgpMessage BGPNotification { get; set; }
        
        public void Decode(byte[] data)
        {
            Reason = data[0];
        }
    }
}