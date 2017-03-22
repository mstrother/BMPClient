using System;

namespace BmpListener.Bgp
{
    public class PathAttributeCommunity : PathAttribute
    {
        public PathAttributeCommunity(byte[] data, int offset) 
            : base(data, offset)
        {
            Decode(data, Offset);
        }

        public uint Community { get; private set; }

        protected void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 4);
            Community = BitConverter.ToUInt32(data, offset);
        }
    }
}
