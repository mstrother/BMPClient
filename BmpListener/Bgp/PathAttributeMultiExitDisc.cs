using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeMultiExitDisc : PathAttribute
    {
        public PathAttributeMultiExitDisc(ArraySegment<byte> data) : base(data)
        {
            Decode(AttributeValue);
        }

        public int? Metric { get; private set; }

        protected void Decode(ArraySegment<byte> data)
        {
            Array.Reverse(data.Array, data.Offset, 0);
            Metric = BitConverter.ToInt32(data.Array, 0);
        }
    }
}