using System;
using System.Linq;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeNextHop : PathAttribute
    {
        public PathAttributeNextHop(ArraySegment<byte> data) : base(ref data)
        {
            NextHop = new IPAddress(data.ToArray());
        }

        public IPAddress NextHop { get; }
    }
}
