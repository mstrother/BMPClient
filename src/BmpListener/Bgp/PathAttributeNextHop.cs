using System;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeNextHop : PathAttribute
    {
        public PathAttributeNextHop(byte[] data, int offset)
            : base(data, offset)
        {
            Decode(data, Offset);
        }

        public IPAddress NextHop { get; private set; }

        protected void Decode(byte[] data, int offset)
        {
            var ipBytes = new byte[4];
            Array.Copy(data, offset, ipBytes, 0, 4);
            NextHop = new IPAddress(ipBytes);
        }
    }
}
