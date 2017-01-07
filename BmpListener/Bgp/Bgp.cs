using System;

namespace BmpListener.Bgp
{
    public enum AddressFamily
    {
        IPv4 = 1,
        IPv6,
        L2VPN = 25
    }

    [Flags]
    public enum AttributeFlags
    {
        EXTENDED_LENGTH = 1 << 4,
        PARTIAL = 1 << 5,
        TRANSITIVE = 1 << 6,
        OPTIONAL = 1 << 7
    }

    public enum AttributeType
    {
        ORIGIN = 1,
        AS_PATH,
        NEXT_HOP,
        MULTI_EXIT_DISC,
        LOCAL_PREF,
        ATOMIC_AGGREGATE,
        AGGREGATOR,
        COMMUNITY,
        ORIGINATOR_ID,
        CLUSTER_LIST,
        MP_REACH_NLRI = 14,
        MP_UNREACH_NLRI,
        EXTENDED_COMMUNITIES,
        AS4_PATH,
        AS4_AGGREGATOR,
        PMSI_TUNNEL = 22,
        TUNNEL_ENCAP,
        AIGP = 26,
        LARGE_COMMUNITY = 32
    }

    public enum MessageType
    {
        Open = 1,
        Update,
        Notification,
        Keepalive,
        RouteRefresh
    }

    public enum Origin
    {
        IGP,
        EGP,
        Incomplete
    }

    public enum SegmentType
    {
        AS_SET = 1,
        AS_SEQUENCE,
        AS_CONFED_SEQUENCE,
        AS_CONFED_SET
    }

    public enum SubsequentAddressFamily
    {
        Unicast = 1,
        Multicast = 2
    }
}