using System;

namespace BmpListener.Bgp
{
    public class PathAttrAtomicAggregate : PathAttribute
    {
        public PathAttrAtomicAggregate(ArraySegment<byte> data) : base(data)
        {
        }        
    }
}