using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class PathAttributeMPUnreachNLRI : PathAttribute
    {
        public PathAttributeMPUnreachNLRI(byte[] data, int offset)
            : base(data, offset)
        {
            WithdrawnRoutes = new List<IPAddrPrefix>();
            Decode(data, Offset);
        }

        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IList<IPAddrPrefix> WithdrawnRoutes { get; }

        protected void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 2);
            AFI = (AddressFamily)BitConverter.ToInt16(data, offset);
            SAFI = (SubsequentAddressFamily)data[offset + 2];
            offset += 3;

            var maxOffset = Length - offset;
            while (offset < maxOffset)
            {
                var prefix = new IPAddrPrefix(data, offset, AFI);
                offset += prefix.ByteLength;
            }
        }
    }
}

