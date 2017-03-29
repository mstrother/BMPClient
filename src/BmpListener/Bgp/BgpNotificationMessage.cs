using System;

namespace BmpListener.Bgp
{
    public sealed class BgpNotification : BgpMessage
    {
        public override void Decode(byte[] data, int offset)
        {
            throw new NotImplementedException();
        }
    }
}