using System;
using System.Collections.Generic;
using System.Linq;

namespace BMPClient.BGP
{
    public class PathAttributeMPUnreachNLRI : PathAttribute
    {
        public PathAttributeMPUnreachNLRI(ArraySegment<byte> data) : base(data)
        {
        }

        public BGP.AddressFamily AFI { get; private set; }
        public BGP.SubsequentAddressFamily SAFI { get; private set; }
        public IPAddrPrefix[] Value { get; private set; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            var ipAddrPrefixes = new List<IPAddrPrefix>();
            AFI = (BGP.AddressFamily)data.ToUInt16(0);
            SAFI = (BGP.SubsequentAddressFamily)data.ElementAt(2);

            for (var i = 3; i < data.Count;)
            {
                int length = data.ElementAt(i);
                if (length == 0) return;
                length = (length + 7) / 8;
                length++;
                var offset = data.Offset + i;
                var prefixSegment = new ArraySegment<byte>(data.Array, offset, length);
                var ipAddrPrefix = new IPAddrPrefix(prefixSegment, AFI);
                ipAddrPrefixes.Add(ipAddrPrefix);
                i += prefixSegment.Count;
            }

            Value = ipAddrPrefixes.ToArray();
        }
    }
}