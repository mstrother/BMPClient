using System;
using System.Collections.Generic;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeMPReachNLRI : PathAttribute
    {
        int nextHopLength;

        public PathAttributeMPReachNLRI(byte[] data, int offset)
            : base(data, offset)
        {
            NLRI = new List<IPAddrPrefix>();
            Decode(data, Offset);
        }

        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IPAddress NextHop { get; private set; }
        public IPAddress LinkLocalNextHop { get; private set; }
        public IList<IPAddrPrefix> NLRI { get; }

        protected void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 2);
            AFI = (AddressFamily)BitConverter.ToInt16(data, offset);
            SAFI = (SubsequentAddressFamily)data[offset + 2];
            nextHopLength = data[offset + 3];
            offset += 4;

            if (nextHopLength > 0)
            {
                var nextHopBytes = new byte[16];
                Array.Copy(data, offset, nextHopBytes, 0, 16);
                NextHop = new IPAddress(nextHopBytes);
                offset += 16;

                // RFC 2545 
                // The value of the Length of Next Hop Network  Address field
                // on a MP_REACH_NLRI attribute shall be set to 16, when only a
                // global address is present, or 32 if a link - local address 
                // is also included in the Next Hop field.

                if (nextHopLength == 32)
                {
                    var linklocalBytes = new byte[16];
                    Array.Copy(data, offset, linklocalBytes, 0, 16);
                    LinkLocalNextHop = new IPAddress(linklocalBytes);
                    offset += 16;
                }
            }

            // RFC4760 - 1 reserved byte
            offset++;
            SetNlri(data, offset);
        }

        public void SetNlri(byte[] data, int offset)
        {
            // RFC 4760
            // Address Family Identifier (2 octets)
            // Subsequent Address Family Identifier (1 octet)
            // Length of Next Hop Network Address (1 octet)
            // Network Address of Next Hop (variable)
            // Reserved (1 octet)
            // Network Layer Reachability Information (variable)

            for (int i = 0; i < Length - (nextHopLength + 5);)
            {
                var prefix = new IPAddrPrefix(data, offset, AddressFamily.IP6);
                NLRI.Add(prefix);
                offset += prefix.ByteLength;
                i += prefix.ByteLength;
            }
        }
    }
}

