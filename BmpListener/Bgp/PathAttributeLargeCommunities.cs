using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeLargeCommunities : PathAttribute
    {
        public PathAttributeLargeCommunities(ArraySegment<byte> data) : base(ref data)
        {
        }
    }
}