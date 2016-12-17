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
            var ipAddrPrefixes = new List<IPAddrPrefix>();

            AFI = (BGP.AddressFamily) data.ToUInt16(0);
            SAFI = (BGP.SubsequentAddressFamily) data.ElementAt(2);

            var offset = 4;
            var nextHopLength = data.ElementAt(3);

            if (nextHopLength > 0)
            {
                //TODO colalescing
                var addrLength = 4;
                if (AFI == BGP.AddressFamily.IPv6)
                    addrLength = 16;
                NextHop = new IPAddress(data.Skip(offset).Take(addrLength).ToArray());
                offset += addrLength;
            }

            offset++;

            while (offset < data.Count)
            {
                var ipAddrPrefixLen = data.Count - offset;
                var ipAddrPrefixBytes = new byte[ipAddrPrefixLen];
                Buffer.BlockCopy(data.ToArray(), offset, ipAddrPrefixBytes, 0, ipAddrPrefixLen);
                var ipAddrPrefix = new IPAddrPrefix(ipAddrPrefixBytes);
                offset += ipAddrPrefixLen;
                ipAddrPrefixes.Add(ipAddrPrefix);
            }

            Value = ipAddrPrefixes.ToArray();
        }
    }
}