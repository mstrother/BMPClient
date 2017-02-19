using BmpListener.Extensions;
using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public sealed class BgpUpdateMessage : BgpMessage
    {
        public BgpUpdateMessage(ArraySegment<byte> data) : base(ref data)
        {
            Attributes = new List<PathAttribute>();
            NLRI = new List<IPAddrPrefix>();
            WithdrawnRoutes = new List<IPAddrPrefix>();
            DecodeFromBytes(data);
            IsEndofRib = WithdrawnRoutes.Count == 00 && NLRI.Count == 0;
            if (IsEndofRib)
            {
                int i = 0;
            }
        }

        public int WithdrawnRoutesLength { get; private set; }
        public int PathAttributeLength { get; private set; }
        public List<PathAttribute> Attributes { get; private set; }
        public List<IPAddrPrefix> WithdrawnRoutes { get; private set; }
        public List<IPAddrPrefix> NLRI { get; private set; }
        public bool IsEndofRib { get; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            WithdrawnRoutesLength = data.ToInt16(0);
            PathAttributeLength = data.ToInt16(2);
            data = new ArraySegment<byte>(data.Array, data.Offset + 4, data.Count - 4);
            SetwithdrawnRoutes(data);
            SetPathAttributes(data);
            SetNlri(data);
        }

        public void SetwithdrawnRoutes(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            var count = WithdrawnRoutesLength;
            data = new ArraySegment<byte>(data.Array, offset, count);
            while (data.Count > 0)
            {
                var prefix = new IPAddrPrefix(data);
                WithdrawnRoutes.Add(prefix);
                var byteLength = prefix.GetByteLength();
                offset = data.Offset + byteLength;
                count = data.Count - byteLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }

        public void SetPathAttributes(ArraySegment<byte> data)
        {
            var offset = data.Offset + WithdrawnRoutesLength;
            data = new ArraySegment<byte>(data.Array, offset, PathAttributeLength);
            while (data.Count > 0)
            {
                var pathAttribute = PathAttribute.GetPathAttribute(data);
                Attributes.Add(pathAttribute);
                var pathLength = pathAttribute.Length;
                var extLength = pathAttribute.Flags.HasFlag(PathAttribute.AttributeFlags.ExtendedLength);
                pathLength += extLength ? +4 : +3;
                offset = data.Offset + pathLength;
                var count = data.Count - pathLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }

        public void SetNlri(ArraySegment<byte> data)
        {
            var offset = data.Offset + WithdrawnRoutesLength + PathAttributeLength;
            var count = data.Count - WithdrawnRoutesLength - PathAttributeLength;
            data = new ArraySegment<byte>(data.Array, offset, count);
            while (data.Count > 0)
            {
                var prefix = new IPAddrPrefix(data);
                NLRI.Add(prefix);
                var byteLength = prefix.GetByteLength();
                offset = data.Offset + byteLength;
                count = data.Count - byteLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }
    }
}