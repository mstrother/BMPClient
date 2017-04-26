using BmpListener.Utilities;

namespace BmpListener.Bmp
{
    public class BmpTermination : BmpMessage
    {
        public ushort Type { get; private set; }
        public ushort Length { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Type = EndianBitConverter.Big.ToUInt16(data, offset);
            offset += 2;
            Length = EndianBitConverter.Big.ToUInt16(data, offset);
        }
    }
}