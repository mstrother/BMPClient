using System;

namespace BmpListener.Bgp
{
    public sealed class BgpNotification : BgpMessage
    {
        public BgpNotification(BgpHeader bgpHeader, byte[] data) 
            : base(bgpHeader)
        {
            DecodeFromBytes(data);
        }

        public void DecodeFromBytes(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}