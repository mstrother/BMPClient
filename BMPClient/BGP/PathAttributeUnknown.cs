using System;

namespace BmpListener.Bgp
{
    public class PathAttributeUnknown : PathAttribute
    {
        public PathAttributeUnknown(ArraySegment<byte> data) : base(data)
        {
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
        }
    }
}