using System;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeNextHop : PathAttribute
    {
        public PathAttributeNextHop(ArraySegment<byte> data) : base(data)
        {
            Decode(AttributeValue);
        }

        public IPAddress NextHop { get; private set; }

        protected void Decode(ArraySegment<byte> data)
        {
            var ipBytes = new byte[4];
            Array.Copy(data.Array, data.Offset, ipBytes, 0, 4);
            NextHop = new IPAddress(ipBytes);
        }
    }
}
