using System;

namespace BmpListener.Bgp
{
    public class PathAttributeLargeCommunities : PathAttribute
    {
        public int Asn { get; set; }
        public int LocalData1 { get; set; }
        public int LocalData2 { get; set; }

        public override string ToString()
        {
            return ($"{Asn}:{LocalData1}:{LocalData2}");
        }

        public override void Decode(ArraySegment<byte> data)
        {
            Asn = BigEndian.ToInt32(data, 0);
            LocalData1 = BigEndian.ToInt32(data, 4);
            LocalData2 = BigEndian.ToInt32(data, 8);
        }
    }
}