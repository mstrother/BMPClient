using System;

namespace BMPClient.BGP
{
    public class PathAttributeAggregator : PathAttribute
    {
        public PathAttributeAggregator(ArraySegment<byte> data) : base(data)
        {
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
        }
    }
}