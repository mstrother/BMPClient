using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public sealed class BgpUpdateMessage : BgpMessage
    {
        public BgpUpdateMessage(BgpHeader bgpHeader, byte[] data, int offset)
             : base(bgpHeader)
        {
            Attributes = new List<PathAttribute>();
            WithdrawnRoutes = new List<IPAddrPrefix>();
            Nlri = new List<IPAddrPrefix>();
            DecodeFromBytes(data, offset);
        }

        public int WithdrawnRoutesLength { get; private set; }
        public int PathAttributeLength { get; private set; }
        public List<PathAttribute> Attributes { get; }
        public List<IPAddrPrefix> WithdrawnRoutes { get; }
        public List<IPAddrPrefix> Nlri { get; }

        public void DecodeFromBytes(byte[] data, int offset)
        {
            if (Header.Length == 23)
            {
                // End-of-RIB
                return;
            }

            Array.Reverse(data, offset, 2);
            WithdrawnRoutesLength = BitConverter.ToInt16(data, offset);
            offset += WithdrawnRoutesLength + 2;
            if (WithdrawnRoutesLength > 0)
            {
                SetwithdrawnRoutes(data, offset);
            }

            Array.Reverse(data, offset, 2);
            PathAttributeLength = BitConverter.ToInt16(data, offset);
            offset += 2;
            SetPathAttributes(data, offset);
            offset += PathAttributeLength;

            SetNlri(data, offset);
        }

        public void SetwithdrawnRoutes(byte[] data, int offset)
        {
            for (int i = 0; i < WithdrawnRoutesLength;)
            {
                var prefix = new IPAddrPrefix(data, offset);
                WithdrawnRoutes.Add(prefix);
                i += 1 + ((prefix.Length + 7) / 8);
            }
        }

        public void SetPathAttributes(byte[] data, int offset)
        {
            for (int i = 0; i < PathAttributeLength;)
            {
                var attr = PathAttribute.Create(data, offset);
                Attributes.Add(attr);
                i += (data[offset] & (1 << 4)) != 0
                    ? attr.Length + 4 : attr.Length + 3;
            }
        }

        public void SetNlri(byte[] data, int offset)
        {
            // RFC 4721 - The length, in octets, of the Network Layer
            // Reachability Information is not encoded explicitly,
            // but can be calculated as:
            //
            // UPDATE message Length - 23 - Total Path Attributes Length - Withdrawn Routes Length
            //
            // where UPDATE message Length is the value encoded in the fixed-
            // size BGP header, Total Path Attribute Length, and Withdrawn
            // Routes Length are the values encoded in the variable part of
            // the UPDATE message, and 23 is a combined length of the fixed-
            // size BGP header, the Total Path Attribute Length field, and the
            // Withdrawn Routes Length field.

            var length = Header.Length - 23 - WithdrawnRoutesLength - PathAttributeLength;

            for (int i = 0; i < length;)
            {
                var prefix = new IPAddrPrefix(data, offset);
                Nlri.Add(prefix);
                offset += prefix.ByteLength;
                i += prefix.ByteLength;
            }
        }
    }
}


