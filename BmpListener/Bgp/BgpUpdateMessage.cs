using BmpListener.Extensions;
using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public sealed class BgpUpdateMessage : BgpMessage
    {
        private readonly List<PathAttribute> pathAttributes = new List<PathAttribute>();
        private readonly List<IPAddrPrefix> withDrawnRoutes = new List<IPAddrPrefix>();

        public BgpUpdateMessage(ArraySegment<byte> data) : base(ref data)
        {
            Attributes = new List<PathAttribute>();
            DecodeFromBytes(data);
        }
        
        public List<PathAttribute> Attributes { get; private set; } 

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            var withdrawnRoutesLength = data.ToInt16(0);
            var totalPathAttributeLength = data.ToInt16(2);
            var offset = data.Offset + 4;

            for (int i = withdrawnRoutesLength; i < withdrawnRoutesLength;)
            {
                //var prefix = new byte[5];
                //Buffer.BlockCopy(data.ToArray(), offset, prefix, 0, 5);
                //var segment = new ArraySegment<byte>(data);
                //var ipAddrPrefix = new IPAddrPrefix(prefix);
                //withDrawnRoutes.Add(ipAddrPrefix);
                ////TODO check the prefix length
                //i -= 5;
                //offset += 5;
            }

            for (int i = totalPathAttributeLength; i > 0;)
            {
                var attrBytes = new ArraySegment<byte>(data.Array, offset, i);
                var pathAttribute = PathAttribute.GetPathAttribute(attrBytes);
                if (pathAttribute.Flags.HasFlag(PathAttribute.AttributeFlags.ExtendedLength))
                {
                    offset += (int)pathAttribute.Length + 4;
                    i -= (int)pathAttribute.Length + 4;
                }
                else
                {
                    offset += (int)pathAttribute.Length + 3;
                    i -= (int)pathAttribute.Length + 3;
                }
                Attributes.Add(pathAttribute);
            }
            
            var nlriLength = data.Array.Length - 23 - totalPathAttributeLength - withdrawnRoutesLength;
        }
    }
}