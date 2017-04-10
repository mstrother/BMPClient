using BmpListener.MiscUtil.Conversion;
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

        public override void Decode(byte[] data, int offset)
        {
            Asn = EndianBitConverter.Big.ToInt32(data, offset);
            LocalData1 = EndianBitConverter.Big.ToInt32(data, offset + 4);
            LocalData2 = EndianBitConverter.Big.ToInt32(data, offset + 8);
        }
    }
}