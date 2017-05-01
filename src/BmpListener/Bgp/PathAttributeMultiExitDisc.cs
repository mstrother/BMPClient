using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    internal class PathAttributeMultiExitDisc : PathAttribute
    {
        public uint Metric { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Metric = EndianBitConverter.Big.ToUInt32(data, offset);
        }
    }
}