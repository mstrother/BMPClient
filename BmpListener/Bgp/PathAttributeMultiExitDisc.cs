using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeMultiExitDisc : PathAttribute
    {
        public PathAttributeMultiExitDisc(ArraySegment<byte> data) : base(data)
        {
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
        }
    }
}