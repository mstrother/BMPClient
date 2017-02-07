namespace BmpListener.Bgp
{
    public enum PathAttributeType
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
}
