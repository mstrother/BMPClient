using System;

namespace BMPClient.BGP
{
    internal class PathAttributeLargeCommunities : PathAttribute
    {
        public PathAttributeLargeCommunities(ArraySegment<byte> data) : base(data)
        {
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
        }
    }
}