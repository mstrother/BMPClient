using System;
using System.Collections.Generic;

namespace BMPClient.BGP
{
    public class BGPUpdate : ImsgBody
    {
        private readonly List<PathAttribute> pathAttributes = new List<PathAttribute>();
        
        public ushort WithdrawnRoutesLength { get; private set; }
        public IPAddrPrefix[] WithdrawnRoutes { get; private set; }
        public ushort TotalPathAttributeLength { get; private set; }

        public PathAttribute[] PathAttributes => pathAttributes.ToArray();


        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            WithdrawnRoutesLength = data.ToUInt16(0);
            TotalPathAttributeLength = data.ToUInt16(2);
            var offset = 23;

            for (int i = WithdrawnRoutesLength; i > 0;)
            {
            }

            for (int i = TotalPathAttributeLength; i > 0;)
            {
                data = new ArraySegment<byte>(data.Array, offset, i);
                var pathAttribute = PathAttribute.GetPathAttribute(data);
                offset += (int) pathAttribute.Length + 3;
                pathAttributes.Add(pathAttribute);
                i -= (int) pathAttribute.Length + 3;
            }
        }
    }
}