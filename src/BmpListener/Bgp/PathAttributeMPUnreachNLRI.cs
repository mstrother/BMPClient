using BmpListener.MiscUtil.Conversion;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class PathAttributeMPUnreachNlri : PathAttribute
    {
        public AddressFamily Afi { get; private set; }
        public SubsequentAddressFamily Safi { get; private set; }
        public IList<IPAddrPrefix> WithdrawnRoutes { get; } = new List<IPAddrPrefix>();

        public override void Decode(byte[] data, int offset)
        {
            Afi = (AddressFamily)EndianBitConverter.Big.ToInt16(data, offset);
            offset += 2;
            Safi = (SubsequentAddressFamily)data[offset];
            offset++;

            for (int i = 0; i < Length - 3;)
            {
                (IPAddrPrefix prefix, int length) = IPAddrPrefix.Decode(data, offset + i, Afi);
                WithdrawnRoutes.Add(prefix);
                i += length;
            }
        }
    }
}
