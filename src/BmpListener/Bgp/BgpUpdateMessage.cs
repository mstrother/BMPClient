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

        public override void Decode(byte[] data, int offset)
        {
            if (data.Length == 23)
            {
                // End-of-RIB
            }

            WithdrawnRoutesLength = EndianBitConverter.Big.ToInt16(data, offset);
            offset += 2;
            SetwithdrawnRoutes(data, offset);
            offset += WithdrawnRoutesLength;

            PathAttributeLength = EndianBitConverter.Big.ToInt16(data, offset);
            offset += 2;
            SetPathAttributes(data, offset);

            offset += PathAttributeLength;
            var nlriLength = Header.Length - 23 - PathAttributeLength - WithdrawnRoutesLength;
            SetNlri(data, offset, nlriLength);
        }


        private void SetwithdrawnRoutes(byte[] data, int offset)
        {
            for (var i = 0; i < WithdrawnRoutesLength;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, offset + i, AddressFamily.IP);
                WithdrawnRoutes.Add(prefix);
                i += byteLength;
            }
        }

        private void SetPathAttributes(byte[] data, int offset)
        {
            for (var i = 0; i < PathAttributeLength;)
            {
                (PathAttribute attr, int length) = PathAttribute.DecodeAttribute(data, offset);
                Attributes.Add(attr);
                offset += length;
                i += length;
            }
        }

        private void SetNlri(byte[] data, int offset, int length)
        {
            for (var i = 0; i < length;)
            {
                (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, offset + i, AddressFamily.IP);
                Nlri.Add(prefix);
                offset += byteLength;
                i += byteLength;
            }
        }
    }
}

