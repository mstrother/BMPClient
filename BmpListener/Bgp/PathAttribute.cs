using System;

namespace BmpListener.Bgp
{
    public abstract class PathAttribute
    {
        protected PathAttribute(byte[] data, int offset)
        {
            Flags = (AttributeFlags)data[offset];
            AttributeType = (PathAttributeType)data[offset + 1];
            bool extLength = (data[offset] & (1 << 4)) != 0;
            if (extLength)
            {
                Array.Reverse(data, offset + 2, 2);
                Length = BitConverter.ToUInt16(data, offset + 2);
            }
            else
            {
                Length = data[offset + 2];
            }
            offset = extLength ? offset += 2 : offset += 1;
        }

        [Flags]
        public enum AttributeFlags
        {
            ExtendedLength = 1 << 4,
            Partial = 1 << 5,
            Transitive = 1 << 6,
            Optional = 1 << 7
        }

        protected int Offset { get; }

        public AttributeFlags Flags { get; private set; }
        public PathAttributeType AttributeType { get; private set; }
        public int Length { get; private set; }

        public static PathAttribute Create(byte[] data, int offset)
        {
            var attributeType = (PathAttributeType)data[offset + 1];

            switch (attributeType)
            {
                case PathAttributeType.ORIGIN:
                    return new PathAttributeOrigin(data, offset);
                case PathAttributeType.AS_PATH:
                    return new PathAttributeASPath(data, offset);
                case PathAttributeType.NEXT_HOP:
                    return new PathAttributeNextHop(data, offset);
                case PathAttributeType.MULTI_EXIT_DISC:
                    return new PathAttributeMultiExitDisc(data, offset);
                case PathAttributeType.LOCAL_PREF:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.ATOMIC_AGGREGATE:
                    return new PathAttrAtomicAggregate(data, offset);
                case PathAttributeType.AGGREGATOR:
                    return new PathAttributeAggregator(data, offset);
                case PathAttributeType.COMMUNITY:
                    return new PathAttributeCommunity(data, offset);
                case PathAttributeType.ORIGINATOR_ID:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.CLUSTER_LIST:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.MP_REACH_NLRI:
                    return new PathAttributeMPReachNLRI(data, offset);
                case PathAttributeType.MP_UNREACH_NLRI:
                    return new PathAttributeMPUnreachNLRI(data, offset);
                case PathAttributeType.EXTENDED_COMMUNITIES:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.AS4_PATH:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.AS4_AGGREGATOR:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.PMSI_TUNNEL:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.TUNNEL_ENCAP:
                    return new PathAttributeUnknown(data, offset);
                case PathAttributeType.LARGE_COMMUNITY:
                    return new PathAttributeLargeCommunities(data, offset);
                default:
                    return new PathAttributeUnknown(data, offset);
            }
        }
    }
}