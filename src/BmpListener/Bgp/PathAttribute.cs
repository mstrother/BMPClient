using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public abstract class PathAttribute
    {
        [Flags]
        public enum AttributeFlags
        {
            ExtendedLength = 1 << 4,
            Partial = 1 << 5,
            Transitive = 1 << 6,
            Optional = 1 << 7
        }

        public AttributeFlags Flags { get; private set; }
        public PathAttributeType AttributeType { get; private set; }
        public int Length { get; private set; }

        public virtual void Decode(ArraySegment<byte> data)
        {
        }

        public static (PathAttribute, int) DecodeAttribute(ArraySegment<byte> data)
        {
            var attributeType = (PathAttributeType) data.ElementAt(1);
            PathAttribute attr;

            switch (attributeType)
            {
                case PathAttributeType.ORIGIN:
                    attr = new PathAttributeOrigin();
                    break;
                case PathAttributeType.AS_PATH:
                    attr = new PathAttributeASPath();
                    break;
                case PathAttributeType.NEXT_HOP:
                    attr = new PathAttributeNextHop();
                    break;
                case PathAttributeType.MULTI_EXIT_DISC:
                    attr = new PathAttributeMultiExitDisc();
                    break;
                case PathAttributeType.LOCAL_PREF:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.ATOMIC_AGGREGATE:
                    attr = new PathAttrAtomicAggregate();
                    break;
                case PathAttributeType.AGGREGATOR:
                    attr = new PathAttributeAggregator();
                    break;
                case PathAttributeType.COMMUNITY:
                    attr = new PathAttributeCommunity();
                    break;
                case PathAttributeType.ORIGINATOR_ID:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.CLUSTER_LIST:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.MP_REACH_NLRI:
                    attr = new PathAttributeMPReachNLRI();
                    break;
                case PathAttributeType.MP_UNREACH_NLRI:
                    attr = new PathAttributeMPUnreachNLRI();
                    break;
                case PathAttributeType.EXTENDED_COMMUNITIES:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.AS4_PATH:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.AS4_AGGREGATOR:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.PMSI_TUNNEL:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.TUNNEL_ENCAP:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.LARGE_COMMUNITY:
                    attr = new PathAttributeLargeCommunities();
                    break;
                default:
                    attr = new PathAttributeUnknown();
                    break;
            }

            attr.Flags = (AttributeFlags) data.First();
            attr.AttributeType = (PathAttributeType) data.ElementAt(1);

            var extendedLength = (attr.Flags & AttributeFlags.ExtendedLength) == AttributeFlags.ExtendedLength;

            if (extendedLength)
            {
                attr.Length = BigEndian.ToUInt16(data, 2);
                data = new ArraySegment<byte>(data.Array, data.Offset + 4, attr.Length);
            }
            else
            {
                attr.Length = data.ElementAt(2);
                data = new ArraySegment<byte>(data.Array, data.Offset + 3, attr.Length);
            }
            
            attr.Decode(data);

            var length = extendedLength
                ? attr.Length + 4
                : attr.Length + 3;

            return (attr, length);
        }
    }
}