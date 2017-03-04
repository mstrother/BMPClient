using System;
using System.Collections.Generic;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeMPReachNLRI : PathAttribute
    {
        public PathAttributeMPReachNLRI(ArraySegment<byte> data) : base(data)
        {
            NLRI = new List<IPAddrPrefix>();
            Decode(AttributeValue);
        }

        public AddressFamily AFI { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }
        public IPAddress NextHop { get; private set; }
        public IPAddress LinkLocalNextHop { get; private set; }
        public IList<IPAddrPrefix> NLRI { get; }

        protected void Decode(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            Array.Reverse(data.Array, offset, 2);
            AFI = (AddressFamily)BitConverter.ToInt16(data.Array, offset);
            SAFI = (SubsequentAddressFamily)data.Array[offset + 2];
            var nextHopLength = data.Array[offset + 3];
            offset += 4;

            if (nextHopLength > 0)
            {
                var nextHopBytes = new byte[16];
                Array.Copy(data.Array, offset, nextHopBytes, 0, nextHopLength);
                NextHop = new IPAddress(nextHopBytes);

                // RFC 2545 - The value of the Length of Next Hop Network 
                // Address field on a MP_REACH_NLRI attribute shall be set to
                // 16, when only a global address is present, or 32 if a
                // link - local address is also included in the Next Hop field.

                if (nextHopLength == 32)
                {
                    var linklocalBytes = new byte[16];
                    Array.Copy(data.Array, 16, linklocalBytes, 0, 16);
                    LinkLocalNextHop = new IPAddress(linklocalBytes);
                    offset += 16;
                }
            }

            // RFC4760 - 1 byte reserved byte
            var count = data.Array.Length - (offset + 1);
            var nlriData = new ArraySegment<byte>(data.Array, offset, count);
            SetNlri(nlriData);
        }

        public void SetNlri(ArraySegment<byte> data)
        {
            while (data.Count > 0)
            {
                var prefix = new IPAddrPrefix(data);
                var offset = data.Offset + prefix.ByteLength;
                var count = data.Array.Length - offset - prefix.ByteLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }
    }
}