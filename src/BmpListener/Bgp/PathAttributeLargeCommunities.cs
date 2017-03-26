using System;

namespace BmpListener.Bgp
{
    public class PathAttributeLargeCommunities : PathAttribute
    {
        public PathAttributeLargeCommunities(byte[] data, int offset) : base(data, offset)
        {
            Decode(data, Offset);
        }

        public int Asn { get; set; }
        public int LocalData1 { get; set; }
        public int LocalData2 { get; set; }

        public override string ToString()
        {
            return ($"{Asn}:{LocalData1}:{LocalData2}");
        }

        protected void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 4);
            Asn = BitConverter.ToInt32(data, offset);

            Array.Reverse(data, offset + 4, 4);
            LocalData1 = BitConverter.ToInt32(data, offset);

            Array.Reverse(data, offset + 8, 4);
            LocalData2 = BitConverter.ToInt32(data, offset);
        }
    }
}