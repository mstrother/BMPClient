using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeMultiExitDisc : PathAttribute
    {
        public PathAttributeMultiExitDisc(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromBytes(data);
        }

        public int Metric { get; private set; }

        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            Metric = data.ToInt32(0);
        }
    }
}