using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeMPReachNLRI : PathAttribute
    {
        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IPAddress NextHop { get; private set; }
        public IPAddress LinkLocalNextHop { get; private set; }
        public IList<IPAddrPrefix> NLRI { get; } = new List<IPAddrPrefix>();

        public override void Decode(ArraySegment<byte> data)
        {
            AFI = (AddressFamily)EndianBitConverter.Big.ToInt16(data, 0);
            SAFI = (SubsequentAddressFamily)data.ElementAt(2);
            var nextHopLength = data.ElementAt(3);
            //offset += 4;

            if (nextHopLength > 0)
            {
                var nextHopBytes = new byte[16];
                Array.Copy(data.Array, data.Offset + 4, nextHopBytes, 0, 16);
                NextHop = new IPAddress(nextHopBytes);
                //offset += 16;

                // RFC 2545 
                // The value of the Type of Next Hop Network  Address field
                // on a MP_REACH_NLRI attribute shall be set to 16, when only a
                // global address is present, or 32 if a link - local address 
                // is also included in the Next Hop field.

                //if (nextHopLength == 32)
                //{
                //    var linklocalBytes = new byte[16];
                //    Array.Copy(data.Array, offset, linklocalBytes, 0, 16);
                //    LinkLocalNextHop = new IPAddress(linklocalBytes);
                //}
            }

            // RFC4760 - 1 reserved byte
            //offset++;
            var offset = data.Offset + nextHopLength + 5;
            var count = data.Count - nextHopLength - 5;
            data = new ArraySegment<byte>(data.Array, offset, count);
            SetNlri(data);
        }

        public void SetNlri(ArraySegment<byte> data)
        {
            // RFC 4760
            // Address Family Identifier (2 octets)
            // Subsequent Address Family Identifier (1 octet)
            // Type of Next Hop Network Address (1 octet)
            // Network Address of Next Hop (variable)
            // Reserved (1 octet)
            // Network Layer Reachability Information (variable)

            for (var i = 0; i < data.Count;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, i, AFI);
                NLRI.Add(prefix);
                i += byteLength;
            }
        }
    }
}
