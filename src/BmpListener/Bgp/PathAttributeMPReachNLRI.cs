using System;
using System.Collections.Generic;
using System.Net;
using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    public class PathAttributeMPReachNlri : PathAttribute
    {
        public AddressFamily Afi { get; private set; }
        public SubsequentAddressFamily Safi { get; private set; }
        public IPAddress NextHop { get; private set; }
        public IPAddress LinkLocalNextHop { get; private set; }
        public IList<IPAddrPrefix> Nlri { get; } = new List<IPAddrPrefix>();

        public override void Decode(byte[] data, int offset)
        {
            Afi = (AddressFamily)EndianBitConverter.Big.ToInt16(data, offset);
            offset += 2;
            Safi = (SubsequentAddressFamily)data[offset];
            offset++;
            var nextHopLength = data[offset];
            offset++;

            if (nextHopLength > 0)
            {
                var nextHopBytes = new byte[16];
                Array.Copy(data, offset, nextHopBytes, 0, 16);
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
                offset += nextHopLength;
            }

            // RFC4760 - 1 reserved byte
            offset++;

            for (int i = 0; i < Length - (5 + nextHopLength);)
            {
                (IPAddrPrefix prefix, int length) = IPAddrPrefix.Decode(data, offset + i, Afi);
                Nlri.Add(prefix);
                i += length;
            }
        }
    }
}
