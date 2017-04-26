using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    public class PathAttributeCommunity : PathAttribute
    {
        public uint Community { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Community = EndianBitConverter.Big.ToUInt32(data, offset);
        }

        public override string ToString()
        {
            return $"{Community >> 16}:{Community & 0xffff}";
        }
    }
}
