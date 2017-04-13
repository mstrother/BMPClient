using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeMPUnreachNLRI : PathAttribute
    {
        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IList<IPAddrPrefix> WithdrawnRoutes { get; } = new List<IPAddrPrefix>();

        public override void Decode(byte[] data, int offset)
        {
            AFI = (AddressFamily)EndianBitConverter.Big.ToInt16(data, offset);
            offset++;
            SAFI = (SubsequentAddressFamily)data.ElementAt(offset);
            offset++;

            for (var i = 3; i < Length;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, offset + i, AFI);
                WithdrawnRoutes.Add(prefix);
                offset += byteLength;
                i += byteLength;
            }
        }
    }
}
