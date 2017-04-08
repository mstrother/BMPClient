using BmpListener.MiscUtil.Conversion;
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

        public override void Decode(ArraySegment<byte> data)
        {
            if (data.Count == 23)
            {
                // End-of-RIB
            }

            int offset = data.Offset;
            int count = data.Count;

            WithdrawnRoutesLength = EndianBitConverter.Big.ToInt16(data, 0);
            offset += 2;
            count -= 2;
            data = new ArraySegment<byte>(data.Array, offset, count);
            SetwithdrawnRoutes(data);
            offset += WithdrawnRoutesLength;
            count -= WithdrawnRoutesLength;

            data = new ArraySegment<byte>(data.Array, offset, PathAttributeLength);
            PathAttributeLength = EndianBitConverter.Big.ToInt16(data, 0);
            offset += 2;
            count -= 2;
            data = new ArraySegment<byte>(data.Array, offset, count);
            SetPathAttributes(data);

            offset += PathAttributeLength;
            count -= PathAttributeLength;
            data = new ArraySegment<byte>(data.Array, offset, count);
            SetNlri(data);
        }


        private void SetwithdrawnRoutes(ArraySegment<byte> data)
        {
            for (var i = 0; i < WithdrawnRoutesLength;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, i, AddressFamily.IP);
                WithdrawnRoutes.Add(prefix);
                i += byteLength;
            }
        }

        private void SetPathAttributes(ArraySegment<byte> data)
        {
            for (var i = 0; i < PathAttributeLength;)
            {
                (PathAttribute attr, int length) = PathAttribute.DecodeAttribute(data);
                Attributes.Add(attr);
                data = new ArraySegment<byte>(data.Array, data.Offset + length, data.Count - length);
                i += length;
            }
        }

        private void SetNlri(ArraySegment<byte> data)
        {
            var nlriLength = data.Count;
            for (var i = 0; i < nlriLength;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, i, AddressFamily.IP);
                Nlri.Add(prefix);
                data = new ArraySegment<byte>(data.Array, data.Offset + byteLength, data.Count - byteLength);
                i += byteLength;
            }
        }
    }
}

