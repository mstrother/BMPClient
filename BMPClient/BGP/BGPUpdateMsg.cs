using System;
using System.Collections.Generic;

namespace BmpListener.BGP
{
    public class BGPUpdateMsg : ImsgBody
    {
        private readonly List<PathAttribute> pathAttributes = new List<PathAttribute>();
        private readonly List<IPAddrPrefix> withDrawnRoutes = new List<IPAddrPrefix>();


        public ushort WithdrawnRoutesLength { get; private set; }
        public IPAddrPrefix[] WithdrawnRoutes => withDrawnRoutes.ToArray();
        public ushort TotalPathAttributeLength { get; private set; }
        public PathAttribute[] PathAttributes => pathAttributes.ToArray();
        public IPAddrPrefix[] NLRI { get; private set; }


        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            WithdrawnRoutesLength = data.ToUInt16(0);
            TotalPathAttributeLength = data.ToUInt16(2);
            var offset = 23;
            
            for (int i = WithdrawnRoutesLength; i < WithdrawnRoutesLength;)
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

            for (int i = TotalPathAttributeLength; i > 0;)
            {
                var attrBytes = new ArraySegment<byte>(data.Array, offset, i);
                var pathAttribute = PathAttribute.GetPathAttribute(attrBytes);
                //TODO handle extended length attributes
                offset += (int)pathAttribute.Length + 3;
                pathAttributes.Add(pathAttribute);
                i -= (int)pathAttribute.Length + 3;
            }

            var nlriLength = data.Array.Length - 23 - TotalPathAttributeLength - WithdrawnRoutesLength;
        }
    }
}