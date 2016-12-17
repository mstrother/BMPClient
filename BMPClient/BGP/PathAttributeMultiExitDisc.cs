using System;

namespace BMPClient.BGP
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