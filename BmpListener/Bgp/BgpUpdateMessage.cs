using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace BmpListener.Bgp
{
    public sealed class BgpUpdateMessage : BgpMessage
    {
        private readonly List<PathAttribute> pathAttributes = new List<PathAttribute>();
        private readonly List<IPAddrPrefix> withDrawnRoutes = new List<IPAddrPrefix>();

        public BgpUpdateMessage(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromBytes(data);
        }
        
        public Dictionary<AttributeType, PathAttribute> Attributes { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Announce { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Withdraw { get; private set; }


        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            var withdrawnRoutesLength = data.ToUInt16(0);
            var totalPathAttributeLength = data.ToUInt16(2);
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

            Attributes = new Dictionary<AttributeType, PathAttribute>();
            for (int i = totalPathAttributeLength; i > 0;)
            {
                var attrBytes = new ArraySegment<byte>(data.Array, offset, i);
                var pathAttribute = PathAttribute.GetPathAttribute(attrBytes);
                if (pathAttribute.Flags.HasFlag(AttributeFlags.ExtendedLength))
                {
                    offset += (int)pathAttribute.Length + 4;
                    i -= (int)pathAttribute.Length + 4;
                }
                else
                {
                    offset += (int)pathAttribute.Length + 3;
                    i -= (int)pathAttribute.Length + 3;
                }
                Attributes.Add(pathAttribute.Type, pathAttribute);
            }

            var nlriLength = data.Array.Length - 23 - totalPathAttributeLength - withdrawnRoutesLength;
        }
    }
}