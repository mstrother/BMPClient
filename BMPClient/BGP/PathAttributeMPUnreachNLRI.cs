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

            AFI = (BGP.AddressFamily) data.ToUInt16(0);
            SAFI = (BGP.SubsequentAddressFamily) data.ElementAt(2);

            var offset = 3;

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