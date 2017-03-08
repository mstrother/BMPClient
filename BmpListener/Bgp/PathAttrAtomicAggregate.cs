using System;

namespace BmpListener.Bgp
{
    public class PathAttrAtomicAggregate : PathAttribute
    {
        public PathAttrAtomicAggregate(byte[] data, int offset)
            : base(data, offset)
        {
        }
    }
}