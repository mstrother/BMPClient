using System;

namespace BmpListener.Bgp
{
    public class PathAttributeLargeCommunities : PathAttribute
    {
        public PathAttributeLargeCommunities(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromByes(data);
        }

        public void DecodeFromByes(ArraySegment<byte> data)
        {
            Asn = data.ToInt32(0);
            LocalData1 = data.ToInt32(4);
            LocalData2 = data.ToInt32(8);
        }

        public int Asn { get; set; }
        public int LocalData1 { get; set; }
        public int LocalData2 { get; set; }

        public override string ToString()
        {
            return ($"{Asn}:{LocalData1}:{LocalData2}");
        }
    }
}