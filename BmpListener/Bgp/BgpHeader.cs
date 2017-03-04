using System;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader(ArraySegment<byte> data)
        {
            Decode(data);
        }

        public int Length { get; private set; }
        public BgpMessage.Type Type { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            Array.Reverse(data.Array, data.Offset + 16, 2);
            Length = BitConverter.ToInt16(data.Array, data.Offset + 16);
            Type = (BgpMessage.Type)data.Array[data.Offset + 18];
        }
    }
}