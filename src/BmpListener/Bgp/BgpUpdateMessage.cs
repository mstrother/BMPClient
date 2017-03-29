using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class BgpUpdateMessage : BgpMessage
    {
        public int WithdrawnRoutesLength { get; private set; }
        public int PathAttributeLength { get; private set; }
        public IList<PathAttribute> Attributes { get; } = new List<PathAttribute>();
        public IList<IPAddrPrefix> WithdrawnRoutes { get; } = new List<IPAddrPrefix>();
        public IList<IPAddrPrefix> Nlri { get; } = new List<IPAddrPrefix>();

        public override void Decode(byte[] data, int offset)
        {
            if (Header.Length == 23)
            {
                // End-of-RIB
                return;
            }

            Array.Reverse(data, offset, 2);
            WithdrawnRoutesLength = BitConverter.ToInt16(data, offset);
            offset += 2;
            SetwithdrawnRoutes(data, offset);
            offset += WithdrawnRoutesLength;

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
                offset += prefix.ByteLength;
                i += prefix.ByteLength;
            }
        }

        public void SetPathAttributes(byte[] data, int offset)
        {
            for (int i = 0; i < PathAttributeLength;)
            {
                var attr = PathAttribute.Create(data, offset + i);
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


