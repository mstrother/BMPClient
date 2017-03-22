using System;

namespace BmpListener.Bgp
{
    public class BgpKeepAliveMessage : BgpMessage
    {
        public BgpKeepAliveMessage(byte[] data, int offset)
            : base(data, offset)
        {
        }
    }
}
