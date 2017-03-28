using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader(byte[] data, int offset)
        {
            Decode(data, offset);
        }

        public byte[] Marker { get; } = new byte[16];
        public int Length { get; private set; }
        public BgpMessageType Type { get; private set; }

        public void Decode(byte[] data, int offset)
        {
            Array.Copy(data, offset, Marker, 0, 16);
            Array.Reverse(data, offset + 16, 2);
            Length = BitConverter.ToInt16(data, offset + 16);
            Type = (BgpMessageType)data[offset + 18];
        }
    }
}