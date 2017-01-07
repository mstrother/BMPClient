using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bgp
{
    public abstract class PathAttribute
    {
        protected PathAttribute(ArraySegment<byte> data)
        {
            Flags = (AttributeFlags) data.ElementAt(0);
            Type = (AttributeType) data.ElementAt(1);

            if (Flags.HasFlag(AttributeFlags.EXTENDED_LENGTH))
            {
                if (data.Count < 4)
                    throw new Exception();
                Length = data.ToUInt16(2);
                data = new ArraySegment<byte>(data.Array, data.Offset + 4, (int) Length);
            }
            else
            {
                Length = data.ElementAt(2);
                data = new ArraySegment<byte>(data.Array, data.Offset + 3, (int) Length);
            }
            //TODO validate flags            
            DecodeFromBytes(data);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public AttributeFlags Flags { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AttributeType Type { get; }

        [JsonIgnore]
        public uint Length { get; set; }

        public abstract void DecodeFromBytes(ArraySegment<byte> data);

        public static PathAttribute GetPathAttribute(ArraySegment<byte> data)
        {
            switch ((AttributeType) data.ElementAt(1))
            {
                case AttributeType.ORIGIN:
                    return new PathAttributeOrigin(data);
                case AttributeType.AS_PATH:
                    return new PathAttributeASPath(data);
                case AttributeType.NEXT_HOP:
                    return new PathAttributeUnknown(data);
                case AttributeType.MULTI_EXIT_DISC:
                    return new PathAttributeMultiExitDisc(data);
                case AttributeType.LOCAL_PREF:
                    return new PathAttributeUnknown(data);
                case AttributeType.ATOMIC_AGGREGATE:
                    return new PathAttrAtomicAggregate(data);
                case AttributeType.AGGREGATOR:
                    return new PathAttributeAggregator(data);
                case AttributeType.COMMUNITY:
                    return new PathAttributeUnknown(data);
                case AttributeType.ORIGINATOR_ID:
                    return new PathAttributeUnknown(data);
                case AttributeType.CLUSTER_LIST:
                    return new PathAttributeUnknown(data);
                case AttributeType.MP_REACH_NLRI:
                    return new PathAttributeMPReachNLRI(data);
                case AttributeType.MP_UNREACH_NLRI:
                    return new PathAttributeMPUnreachNLRI(data);
                case AttributeType.EXTENDED_COMMUNITIES:
                    return new PathAttributeUnknown(data);
                case AttributeType.AS4_PATH:
                    return new PathAttributeUnknown(data);
                case AttributeType.AS4_AGGREGATOR:
                    return new PathAttributeUnknown(data);
                case AttributeType.PMSI_TUNNEL:
                    return new PathAttributeUnknown(data);
                case AttributeType.TUNNEL_ENCAP:
                    return new PathAttributeUnknown(data);
                case AttributeType.LARGE_COMMUNITY:
                    return new PathAttributeLargeCommunities(data);
                default:
                    return new PathAttributeUnknown(data);
            }
        }
    }
}