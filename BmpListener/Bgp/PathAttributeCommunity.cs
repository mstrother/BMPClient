using System;

namespace BmpListener.Bgp
{
    public class PathAttributeCommunity : PathAttribute
    {
        public PathAttributeCommunity(ArraySegment<byte> data) : base(data)
        {
            Decode(AttributeValue);
        }

        public uint Community { get; private set; }

        protected void Decode(ArraySegment<byte> data)
        {
            Array.Reverse(data.Array, data.Offset, 4);
            Community = BitConverter.ToUInt32(data.Array, data.Offset);
        }
    }
}
