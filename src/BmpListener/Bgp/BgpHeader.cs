using BmpListener.MiscUtil.Conversion;
using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader()
        {
        }

        public BgpHeader(ArraySegment<byte> data)
        {
            Decode(data);
        }

        public byte[] Marker { get; } = new byte[16];
        public int Length { get; private set; }
        public BgpMessageType Type { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            Length = EndianBitConverter.Big.ToUInt16(data, 16);
            Type = (BgpMessageType)data.ElementAt(18);
        }
    }
}