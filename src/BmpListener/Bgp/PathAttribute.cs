using BmpListener.MiscUtil.Conversion;
using System;

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
                case PathAttributeType.Origin:
                    attr = new PathAttributeOrigin();
                    break;
                case PathAttributeType.AsPath:
                    attr = new PathAttributeASPath();
                    break;
                case PathAttributeType.NextHop:
                    attr = new PathAttributeNextHop();
                    break;
                case PathAttributeType.MultiExitDisc:
                    attr = new PathAttributeMultiExitDisc();
                    break;
                case PathAttributeType.LocalPref:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.AtomicAggregate:
                    attr = new PathAttrAtomicAggregate();
                    break;
                case PathAttributeType.Aggregator:
                    attr = new PathAttributeAggregator();
                    break;
                case PathAttributeType.Community:
                    attr = new PathAttributeCommunity();
                    break;
                case PathAttributeType.OriginatorId:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.ClusterList:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.MpReachNlri:
                    attr = new PathAttributeMPReachNlri();
                    break;
                case PathAttributeType.MpUnreachNlri:
                    attr = new PathAttributeMPUnreachNlri();
                    break;
                case PathAttributeType.ExtendedCommunities:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.As4Path:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.As4Aggregator:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.PmsiTunnel:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.TunnelEncap:
                    attr = new PathAttributeUnknown();
                    break;
                case PathAttributeType.LargeCommunity:
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