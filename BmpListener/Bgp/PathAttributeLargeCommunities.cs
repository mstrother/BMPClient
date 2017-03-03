using BmpListener.Extensions;
using System;

namespace BmpListener.Bgp
{
    public class PathAttributeLargeCommunities : PathAttribute
    {
        public PathAttributeLargeCommunities(ArraySegment<byte> data) : base(data)
        {
            Decode(AttributeValue);
        }

        public int Asn { get; set; }
        public int LocalData1 { get; set; }
        public int LocalData2 { get; set; }

        public override string ToString()
        {
            return ($"{Asn}:{LocalData1}:{LocalData2}");
        }

        protected void Decode(ArraySegment<byte> data)
        {
            var offset = data.Offset;

            Array.Reverse(data.Array, offset, 4);
            Asn = BitConverter.ToInt32(data.Array, offset);
            offset += 4;

            Array.Reverse(data.Array, offset, 4);
            LocalData1 = BitConverter.ToInt32(data.Array, offset);
            offset += 4;

            Array.Reverse(data.Array, offset, 4);
            LocalData2 = BitConverter.ToInt32(data.Array, offset);
        }
    }
}