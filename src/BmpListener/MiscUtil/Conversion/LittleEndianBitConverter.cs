namespace BmpListener.MiscUtil.Conversion
{
    public sealed class LittleEndianBitConverter : EndianBitConverter
    {
        public override bool IsLittleEndian()
        {
            return true;
        }

        public override Endianness Endianness => Endianness.LittleEndian;

        protected override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index)
        {
            for (int i = 0; i < bytes; i++)
            {
                buffer[i + index] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
        }

        protected override long FromBytes(byte[] buffer, int startIndex, int bytesToConvert)
        {
            long ret = 0;
            for (int i = 0; i < bytesToConvert; i++)
            {
                ret = unchecked((ret << 8) | buffer[startIndex + bytesToConvert - 1 - i]);
            }
            return ret;
        }
    }
}
