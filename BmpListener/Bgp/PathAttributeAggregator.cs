using System;

namespace BmpListener.Bgp
{
    public class PathAttributeAggregator : PathAttribute
    {
        public PathAttributeAggregator(ArraySegment<byte> data) : base(ref data)
        {
        }
    }
}