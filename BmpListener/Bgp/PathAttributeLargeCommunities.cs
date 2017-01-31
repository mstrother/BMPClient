using System;

namespace BmpListener.Bgp
{
    internal class PathAttributeLargeCommunities : PathAttribute
    {
        private int asn;
        private int data1;
        private int data2;

        public PathAttributeLargeCommunities(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromByes(data);
        }

        public void DecodeFromByes(ArraySegment<byte> data)
        {
            asn = data.ToInt32(0);
            data1 = data.ToInt32(4);
            data2 = data.ToInt32(8);
        }

        public override string ToString()
        {
            return ($"{asn}:{data1}:{data2}");
        }
    }
}