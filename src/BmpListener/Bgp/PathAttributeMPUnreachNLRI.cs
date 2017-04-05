using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeMPUnreachNLRI : PathAttribute
    {
        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IList<IPAddrPrefix> WithdrawnRoutes { get; }

        public override void Decode(ArraySegment<byte> data)
        {
            AFI = (AddressFamily)BigEndian.ToInt16(data, 0);
            SAFI = (SubsequentAddressFamily) data.ElementAt(3);
            
            for (var i = 0; i < data.Count;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, i, AFI);
                WithdrawnRoutes.Add(prefix);
                i += byteLength;
            }
        }
    }
}
