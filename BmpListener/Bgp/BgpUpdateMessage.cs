using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public sealed class BgpUpdateMessage : BgpMessage
    {
        int offset;

        public BgpUpdateMessage(ArraySegment<byte> data) : base(data)
        {
            offset = MessageData.Offset;
            Attributes = new List<PathAttribute>();
            WithdrawnRoutes = new List<IPAddrPrefix>();
            Nlri = new List<IPAddrPrefix>();
            DecodeFromBytes(MessageData);
        }

        public int WithdrawnRoutesLength { get; private set; }
        public int PathAttributeLength { get; private set; }
        public List<PathAttribute> Attributes { get; }
        public List<IPAddrPrefix> WithdrawnRoutes { get; }
        public List<IPAddrPrefix> Nlri { get; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            Array.Reverse(data.Array, offset, 2);
            WithdrawnRoutesLength = BitConverter.ToInt16(data.Array, offset);
            offset += 2;

            if (WithdrawnRoutesLength > 0)
            {
                SetwithdrawnRoutes(data);
            }

            if (offset == data.Array.Length)
            {
                // End-of-RIB
                return;
            }

            Array.Reverse(data.Array, offset, 2);
            PathAttributeLength = BitConverter.ToInt16(data.Array, offset);
            offset += 2;

            data = new ArraySegment<byte>(data.Array, offset, PathAttributeLength);
            SetPathAttributes(data);

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

            var nlriLength = Header.Length - 23 - WithdrawnRoutesLength - PathAttributeLength;
            data = new ArraySegment<byte>(data.Array, offset, nlriLength);
            SetNlri(data);
        }

        public void SetwithdrawnRoutes(ArraySegment<byte> data)
        {
            while (data.Count > 0)
            {
                var prefix = new IPAddrPrefix(data);
                WithdrawnRoutes.Add(prefix);
                offset += prefix.ByteLength;
                var count = data.Count - prefix.ByteLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }

        public void SetPathAttributes(ArraySegment<byte> data)
        {
            while (data.Count > 0)
            {
                var pathAttribute = PathAttribute.Create(data);
                Attributes.Add(pathAttribute);
                var extLength = pathAttribute.Flags.HasFlag(PathAttribute.AttributeFlags.ExtendedLength);
                var pathLength = pathAttribute.Length + (extLength ? +4 : +3);
                offset = data.Offset + pathLength;
                var count = data.Count - pathLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }

        public void SetNlri(ArraySegment<byte> data)
        {
            while (data.Count > 0)
            {
                var prefix = new IPAddrPrefix(data);
                Nlri.Add(prefix);
                offset += prefix.ByteLength;
                var count = data.Count - prefix.ByteLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }
    }
}


