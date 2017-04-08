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

            WithdrawnRoutesLength = EndianBitConverter.Big.ToInt16(data, 0);
            data = new ArraySegment<byte>(data.Array, data.Offset + 2, WithdrawnRoutesLength);
            SetwithdrawnRoutes(data);

            data = new ArraySegment<byte>(data.Array, data.Offset + WithdrawnRoutesLength, data.Count - WithdrawnRoutesLength);
            PathAttributeLength = EndianBitConverter.Big.ToInt16(data, 0);
            data = new ArraySegment<byte>(data.Array, data.Offset + 2, PathAttributeLength);
            SetPathAttributes(data);
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
    }
}

