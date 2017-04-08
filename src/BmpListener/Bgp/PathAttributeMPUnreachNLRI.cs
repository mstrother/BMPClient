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

        public override void Decode(ArraySegment<byte> data)
        {
            AFI = (AddressFamily)EndianBitConverter.Big.ToInt16(data, 0);
            SAFI = (SubsequentAddressFamily) data.ElementAt(2);
            
            for (var i = 0; i < data.Count;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, i, AFI);
                WithdrawnRoutes.Add(prefix);
                i += byteLength;
            }
        }
    }
}
