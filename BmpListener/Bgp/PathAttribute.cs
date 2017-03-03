using System;

namespace BmpListener.Bgp
{
    public abstract class PathAttribute
    {
        protected PathAttribute(ArraySegment<byte> data)
        {
            AttributeValue = SetValue(data);
        }

        [Flags]
        public enum AttributeFlags
        {
            ExtendedLength = 1 << 4,
            Partial = 1 << 5,
            Transitive = 1 << 6,
            Optional = 1 << 7
        }

        protected ArraySegment<byte> AttributeValue { get; private set; }

        public AttributeFlags Flags { get; private set; }
        public PathAttributeType AttributeType { get; private set; }
        public int Length { get; private set; }


        public ArraySegment<byte> SetValue(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            Flags = (AttributeFlags)data.Array[offset];
            AttributeType = (PathAttributeType)data.Array[offset + 1];

            if (Flags.HasFlag(AttributeFlags.ExtendedLength))
            {
                Array.Reverse(data.Array, offset + 2, 2);
                Length = BitConverter.ToInt16(data.Array, offset + 2);
                return new ArraySegment<byte>(data.Array, offset + 4, Length);
            }
            Length = data.Array[offset + 2];
            return new ArraySegment<byte>(data.Array, offset + 3, Length);

        }

        public static PathAttribute Create(ArraySegment<byte> data)
        {
            var offset = data.Offset + 1;
            var attributeType = (PathAttributeType)data.Array[offset];

            switch (attributeType)
            {
                case PathAttributeType.ORIGIN:
                    return new PathAttributeOrigin(data);
                case PathAttributeType.AS_PATH:
                    return new PathAttributeASPath(data);
                case PathAttributeType.NEXT_HOP:
                    return new PathAttributeNextHop(data);
                case PathAttributeType.MULTI_EXIT_DISC:
                    return new PathAttributeMultiExitDisc(data);
                case PathAttributeType.LOCAL_PREF:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.ATOMIC_AGGREGATE:
                    return new PathAttrAtomicAggregate(data);
                case PathAttributeType.AGGREGATOR:
                    return new PathAttributeAggregator(data);
                case PathAttributeType.COMMUNITY:
                    return new PathAttributeCommunity(data);
                case PathAttributeType.ORIGINATOR_ID:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.CLUSTER_LIST:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.MP_REACH_NLRI:
                    return new PathAttributeMPReachNLRI(data);
                case PathAttributeType.MP_UNREACH_NLRI:
                    return new PathAttributeMPUnreachNLRI(data);
                case PathAttributeType.EXTENDED_COMMUNITIES:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.AS4_PATH:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.AS4_AGGREGATOR:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.PMSI_TUNNEL:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.TUNNEL_ENCAP:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.LARGE_COMMUNITY:
                    return new PathAttributeLargeCommunities(data);
                default:
                    return new PathAttributeUnknown(data);
            }
        }
    }
}