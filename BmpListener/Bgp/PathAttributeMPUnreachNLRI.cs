using BmpListener.Extensions;
using BmpListener.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeMPUnreachNLRI : PathAttribute
    {
        public PathAttributeMPUnreachNLRI(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromBytes(data);
        }

        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IPAddrPrefix[] Value { get; private set; }

        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            var ipAddrPrefixes = new List<IPAddrPrefix>();
            AFI = (AddressFamily) data.ToUInt16(0);
            SAFI = (SubsequentAddressFamily) data.ElementAt(2);

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