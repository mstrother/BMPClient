using System;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader(byte[] data, int offset)
        {
            Decode(data, offset);
        }

        public int Length { get; private set; }
        public BgpMessage.Type Type { get; private set; }

        public void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset + 16, 2);
            Length = BitConverter.ToInt16(data, offset + 16);
            Type = (BgpMessage.Type)data[offset + 18];
        }
    }
}