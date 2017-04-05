using System;

namespace BmpListener.Bgp
{
    public sealed class BgpNotificationMessage : BgpMessage
    {
        public override void Decode(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}