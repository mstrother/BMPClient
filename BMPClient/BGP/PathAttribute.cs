using System;
using System.Linq;

namespace BMPClient.BGP
{
    public abstract class PathAttribute
    {
        protected PathAttribute(ArraySegment<byte> data)
        {
            Flags = (BGP.AttributeFlag) data.First();
            Type = (BGP.AttributeType) data.ElementAt(1);

            if (Flags.HasFlag(BGP.AttributeFlag.EXTENDED_LENGTH))
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

        public BGP.AttributeFlag Flags { get; }
        public BGP.AttributeType Type { get; private set; }
        public uint Length { get; set; }

        public abstract void DecodeFromBytes(ArraySegment<byte> data);

        public static PathAttribute GetPathAttribute(ArraySegment<byte> data)
        {
            switch ((BGP.AttributeType) data.ElementAt(1))
            {
                case BGP.AttributeType.ORIGIN:
                    return new PathAttributeOrigin(data);
                case BGP.AttributeType.AS_PATH:
                    return new PathAttributeASPath(data);
                case BGP.AttributeType.NEXT_HOP:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.MULTI_EXIT_DISC:
                    return new PathAttributeMultiExitDisc(data);
                case BGP.AttributeType.LOCAL_PREF:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.ATOMIC_AGGREGATE:
                    return new PathAttrAtomicAggregate(data);
                case BGP.AttributeType.AGGREGATOR:
                    return new PathAttributeAggregator(data);
                case BGP.AttributeType.COMMUNITY:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.ORIGINATOR_ID:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.CLUSTER_LIST:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.MP_REACH_NLRI:
                    return new PathAttributeMPReachNLRI(data);
                case BGP.AttributeType.MP_UNREACH_NLRI:
                    return new PathAttributeMPUnreachNLRI(data);
                case BGP.AttributeType.EXTENDED_COMMUNITIES:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.AS4_PATH:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.AS4_AGGREGATOR:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.PMSI_TUNNEL:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.TUNNEL_ENCAP:
                    return new PathAttributeUnknown(data);
                case BGP.AttributeType.LARGE_COMMUNITY:
                    return new PathAttributeLargeCommunities(data);
                default:
                    return new PathAttributeUnknown(data);
            }
        }
    }
}