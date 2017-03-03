using System;

namespace BmpListener.Bgp
{
    public sealed class BgpNotification : BgpMessage
    {
        public BgpNotification(ArraySegment<byte> data) : base(data)
        {
            DecodeFromBytes(MessageData);
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}