using System.Collections.Generic;
using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    public class BgpUpdateMessage : BgpMessage
    {
        public bool EndOfRib { get; private set; }
        public int WithdrawnRoutesLength { get; private set; }
        public int PathAttributeLength { get; private set; }
        public IList<PathAttribute> Attributes { get; } = new List<PathAttribute>();
        public IList<IPAddrPrefix> WithdrawnRoutes { get; } = new List<IPAddrPrefix>();
        public IList<IPAddrPrefix> Nlri { get; } = new List<IPAddrPrefix>();

        public override void Decode(byte[] data, int offset)
        {
            if (Header.Length == 23)
            {
                EndOfRib = true;
            }

            WithdrawnRoutesLength = EndianBitConverter.Big.ToInt16(data, offset);
            offset += 2;

            for (var i = 0; i < WithdrawnRoutesLength;)
            {
                (IPAddrPrefix prefix, int length) = IPAddrPrefix.Decode(data, offset + i, AddressFamily.IP);
                WithdrawnRoutes.Add(prefix);
                i += length;
            }
            offset += WithdrawnRoutesLength;

            PathAttributeLength = EndianBitConverter.Big.ToInt16(data, offset);
            offset += 2;

            for (var i = 0; i < PathAttributeLength;)
            {
                (PathAttribute attr, int length) = PathAttribute.DecodeAttribute(data, offset + i);
                Attributes.Add(attr);
                i += length;
            }
            offset += PathAttributeLength;

            for (var i = 0; i < Header.Length - 23 - PathAttributeLength - WithdrawnRoutesLength;)
            {
                (IPAddrPrefix prefix, int length) = IPAddrPrefix.Decode(data, offset + i, AddressFamily.IP);
                Nlri.Add(prefix);
                i += length;
            }
        }
    }
}

