using System;

namespace BmpListener.Bgp
{
    public class BgpKeepAliveMessage : BgpMessage
    {
        public BgpKeepAliveMessage(BgpHeader bgpHeader)
            : base(bgpHeader)
        {
        }
    }
}
