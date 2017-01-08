using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bgp
{
    public class PathAttributeMPReachNLRI : PathAttribute
    {
        public PathAttributeMPReachNLRI(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromBytes(data);
        }

        public IPAddress NextHop { get; private set; }
        public IPAddress LinkLocalNextHop { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddressFamily AFI { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SubsequentAddressFamily SAFI { get; private set; }

        public IPAddrPrefix[] Value { get; private set; }

        public void NewPrefixFromRouteFamily()
        {
        }

        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            AFI = (AddressFamily) data.ToUInt16(0);
            SAFI = (SubsequentAddressFamily) data.ElementAt(2);
            int nextHopLength = data.ElementAt(3);
            var offset = 4;

            if (nextHopLength > 0)
            {
                var addrLength = 4;
                if (AFI == AddressFamily.IPv6)
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