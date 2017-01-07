using System;

namespace BmpListener.Bgp
{
    public sealed class BgpNotification : BgpMessage
    {
        public BgpNotification(BgpHeader header, ArraySegment<byte> data) : base(header)
        {
            DecodeFromBytes(data);
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}