using System;

namespace BmpListener.Bgp
{
    public sealed class BgpNotification : BgpMessage
    {
        public BgpNotification(byte[] data, int offset) 
            : base(data, offset)
        {
            offset += Constants.BgpHeaderLength;
            DecodeFromBytes(data);
        }

        public void DecodeFromBytes(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}