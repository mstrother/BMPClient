using System;

namespace BmpListener.Bgp
{
    public class BgpKeepAliveMessage : BgpMessage
    {
        public override void Decode(ArraySegment<byte> data)
        {
        }
    }
}
