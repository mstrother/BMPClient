using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BMPClient.BGP
{
    public class PathAttributeMPReachNLRI : PathAttribute
    {
        public PathAttributeMPReachNLRI(ArraySegment<byte> data) : base(data)
        {
        }

        public IPAddress NextHop { get; private set; }
        public IPAddress LinkLocalNextHop { get; private set; }
        public BGP.AddressFamily AFI { get; private set; }
        public BGP.SubsequentAddressFamily SAFI { get; private set; }
        public IPAddrPrefix[] Value { get; private set; }

        public void NewPrefixFromRouteFamily()
        {
        }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            AFI = (BGP.AddressFamily) data.ToUInt16(0);
            SAFI = (BGP.SubsequentAddressFamily) data.ElementAt(3);
            int nextHopLength = data.ElementAt(3);
            var offset = 4;

            if (nextHopLength > 0)
            {
                var addrLength = 4;
                if (AFI == BGP.AddressFamily.IPv6)
                    addrLength = 16;
                NextHop = new IPAddress(data.Skip(offset).Take(addrLength).ToArray());
                offset += addrLength;
                var hasLinkLocal = nextHopLength == 32;
                if (hasLinkLocal)
                {
                    LinkLocalNextHop = new IPAddress(data.Skip(offset).Take(addrLength).ToArray());
                    offset += 16;
                }
            }

            //reserved byte (RFC4760)
            offset++;

            var ipAddrPrefixes = new List<IPAddrPrefix>();

            while (offset < data.Count)
            {
                int length = data.ElementAt(offset);
                length = (length + 7) / 8;
                length++;
                var segmentOffset = data.Offset + offset;
                var prefixSegment = new ArraySegment<byte>(data.Array, segmentOffset, length);
                var ipAddrPrefix = new IPAddrPrefix(prefixSegment, AFI);
                ipAddrPrefixes.Add(ipAddrPrefix);
                offset += prefixSegment.Count;
            }

            Value = ipAddrPrefixes.ToArray();
        }
    }
}