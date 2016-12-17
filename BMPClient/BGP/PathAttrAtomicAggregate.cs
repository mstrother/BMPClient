using System;

namespace BMPClient.BGP
{
    public class PathAttrAtomicAggregate : PathAttribute
    {
        public PathAttrAtomicAggregate(ArraySegment<byte> data) : base(data)
        {
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
        }
    }
}