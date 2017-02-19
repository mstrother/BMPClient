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
        public IPAddrPrefix[] WithdrawnRoutes { get; private set; }

        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            var ipAddrPrefixes = new List<IPAddrPrefix>();
            AFI = (AddressFamily) data.ToInt16(0);
            SAFI = (SubsequentAddressFamily) data.ElementAt(2);

            data = new ArraySegment<byte>(data.Array, data.Offset + 3, data.Count - 3);

            while (data.Count > 0)
            {
                int length = data.First();
                if (length == 0) return;
                length = 1 + ((length + 7) / 8);
                var ipAddrPrefix = new IPAddrPrefix(data, AFI);
                ipAddrPrefixes.Add(ipAddrPrefix);
                data = new ArraySegment<byte>(data.Array, data.Offset + length, data.Count - length);
            }

            WithdrawnRoutes = ipAddrPrefixes.ToArray();
        }
    }
}