using BmpListener.Extensions;
using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public abstract class PathAttribute
    {
        protected PathAttribute(ref ArraySegment<byte> data)
        {
            Flags = (AttributeFlags)data.ElementAt(0);
            AttributeType = (PathAttributeType)data.ElementAt(1);

            if (Flags.HasFlag(AttributeFlags.ExtendedLength))
            {
                if (data.Count < 4)
                    throw new Exception();
                Length = data.ToInt16(2);
                data = new ArraySegment<byte>(data.Array, data.Offset + 4, (int)Length);
            }
            else
            {
                Length = data.ElementAt(2);
                data = new ArraySegment<byte>(data.Array, data.Offset + 3, (int)Length);
            }
        }
        
        [Flags]
        public enum AttributeFlags
        {
            ExtendedLength = 1 << 4,
            Partial = 1 << 5,
            Transitive = 1 << 6,
            Optional = 1 << 7
        }
        
        public AttributeFlags Flags { get; }
        public PathAttributeType AttributeType { get; }
        public int Length { get; set; }
        
        public static PathAttribute GetPathAttribute(ArraySegment<byte> data)
        {
            switch ((PathAttributeType)data.ElementAt(1))
            {
                case PathAttributeType.ORIGIN:
                    return new PathAttributeOrigin(data);
                case PathAttributeType.AS_PATH:
                    return new PathAttributeASPath(data);
                case PathAttributeType.NEXT_HOP:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.MULTI_EXIT_DISC:
                    return new PathAttributeMultiExitDisc(data);
                case PathAttributeType.LOCAL_PREF:
                    return new PathAttributeUnknown(data);
                case PathAttributeType.ATOMIC_AGGREGATE:
                    return new PathAttrAtomicAggregate(data);
                case PathAttributeType.AGGREGATOR:
                    return new PathAttributeAggregator(data);
                case PathAttributeType.COMMUNITY:
                    return new PathAttributeUnknown(data);
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