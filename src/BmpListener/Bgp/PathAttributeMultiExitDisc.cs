using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeMultiExitDisc : PathAttribute
    {
        public PathAttributeMultiExitDisc(byte[] data, int offset)
            : base(data, offset)
        {
            Decode(data, Offset);
        }

        public int? Metric { get; private set; }

        protected void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 0);
            Metric = BitConverter.ToInt32(data, 0);
        }
    }
}