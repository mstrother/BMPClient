using BmpListener.MiscUtil.Conversion;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader()
        {
        }

        public BgpHeader(byte[] data, int offset)
        {
            Decode(data, offset);
        }

        public byte[] Marker { get; } = new byte[16];
        public int Length { get; private set; } = Constants.BgpHeaderLength;
        public BgpMessageType Type { get; private set; }

        public void Decode(byte[] data, int offset)
        {
            Length = EndianBitConverter.Big.ToUInt16(data, offset + 16);
            Type = (BgpMessageType)data[offset + 18];
        }
    }
}