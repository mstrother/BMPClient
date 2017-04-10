using BmpListener.MiscUtil.Conversion;
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

        public virtual void Decode(byte[] data, int offset)
        {
        }

        public static (PathAttribute, int) DecodeAttribute(byte[] data, int offset)
        {
            var attributeType = (PathAttributeType)data[offset + 1];
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

            attr.Flags = (AttributeFlags)data[offset];
            attr.AttributeType = attributeType;
            offset += 2;

            var extendedLength = (attr.Flags & AttributeFlags.ExtendedLength) == AttributeFlags.ExtendedLength;

            if (extendedLength)
            {
                attr.Length = EndianBitConverter.Big.ToUInt16(data, offset);
                offset += 2;
            }
            else
            {
                attr.Length = data[offset];
                offset++;
            }

            attr.Decode(data, offset);

            var length = extendedLength
                ? attr.Length + 4
                : attr.Length + 3;

            return (attr, length);
        }
    }
}