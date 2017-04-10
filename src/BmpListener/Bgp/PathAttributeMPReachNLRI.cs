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

        public override void Decode(byte[] data, int offset)
        {
            AFI = (AddressFamily)EndianBitConverter.Big.ToInt16(data, offset);
            SAFI = (SubsequentAddressFamily)data.ElementAt(offset + 2);
            var nextHopLength = data.ElementAt(offset + 3);
            //offset += 4;

            if (nextHopLength > 0)
            {
                var nextHopBytes = new byte[16];
                Array.Copy(data, offset + 4, nextHopBytes, 0, 16);
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
            offset += nextHopLength + 5;
            var nlriLength = Length - offset;
            SetNlri(data, offset, nlriLength);
        }

        public void SetNlri(byte[] data, int offset, int length)
        {
            for (var i = 0; i < length;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, i, AFI);
                NLRI.Add(prefix);
                offset += byteLength;
                i += byteLength;
            }
        }
    }
}
