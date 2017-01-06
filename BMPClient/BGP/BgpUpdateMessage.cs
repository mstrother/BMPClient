using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BmpListener.Bgp
{
    public sealed class BgpUpdateMessage : BgpMessage
    {
        public BgpUpdateMessage(BgpHeader header, ArraySegment<byte> data) : base(header)
        {
            DecodeFromBytes(data);
        }

        private readonly List<PathAttribute> pathAttributes = new List<PathAttribute>();
        private readonly List<IPAddrPrefix> withDrawnRoutes = new List<IPAddrPrefix>();

        [JsonIgnore]
        public ushort WithdrawnRoutesLength { get; private set; }

        public IPAddrPrefix[] WithdrawnRoutes => withDrawnRoutes.ToArray();

        [JsonIgnore]
        public ushort TotalPathAttributeLength { get; private set; }

        public PathAttribute[] PathAttributes => pathAttributes.ToArray();
        public IPAddrPrefix[] NLRI { get; private set; }


        public override void DecodeFromBytes(ArraySegment<byte> data)
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
                if (pathAttribute.Flags.HasFlag(AttributeFlags.EXTENDED_LENGTH))
                {
                    offset += (int)pathAttribute.Length + 4;
                    i -= (int)pathAttribute.Length + 4;
                }
                else
                {
                    offset += (int)pathAttribute.Length + 3;
                    i -= (int)pathAttribute.Length + 3;
                }
                pathAttributes.Add(pathAttribute);
            }

            var nlriLength = data.Array.Length - 23 - TotalPathAttributeLength - WithdrawnRoutesLength;
        }
    }
}