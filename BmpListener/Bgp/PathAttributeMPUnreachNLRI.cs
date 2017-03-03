using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class PathAttributeMPUnreachNLRI : PathAttribute
    {
        public PathAttributeMPUnreachNLRI(ArraySegment<byte> data) : base(data)
        {
            WithdrawnRoutes = new List<IPAddrPrefix>();
            Decode(AttributeValue);
        }

        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IList<IPAddrPrefix> WithdrawnRoutes { get; }

        protected void Decode(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            Array.Reverse(data.Array, offset, 2);
            AFI = (AddressFamily)BitConverter.ToInt16(data.Array, offset);          
            SAFI = (SubsequentAddressFamily)data.Array[offset + 2];
            offset += 2;

            var count = data.Array.Length - offset;
            var withdrawnRouteData = new ArraySegment<byte>(data.Array, offset, count);

            while (withdrawnRouteData.Count > 0)
            {
                var prefix = new IPAddrPrefix(withdrawnRouteData);
                offset += prefix.ByteLength;
                count = withdrawnRouteData.Count - prefix.ByteLength;
                withdrawnRouteData = new ArraySegment<byte>(withdrawnRouteData.Array, offset, count);
            }
        }
    }
}

