using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeMultiExitDisc : PathAttribute
    {
        public int? Metric { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            //Array.Reverse(data, offset, 0);
            //Metric = BitConverter.ToInt32(data, 0);
        }
    }
}