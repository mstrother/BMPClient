namespace BmpListener.Bgp
{
    // http://www.iana.org/assignments/bgp-parameters/bgp-parameters.xhtml#bgp-parameters-2
    public enum PathAttributeType
    {
        Origin = 1,
        AsPath,
        NextHop,
        MultiExitDisc,
        LocalPref,
        AtomicAggregate,
        Aggregator,
        Community,
        OriginatorId,
        ClusterList,
        MpReachNlri = 14,
        MpUnreachNlri,
        ExtendedCommunities,
        As4Path,
        As4Aggregator,
        PmsiTunnel = 22,
        TunnelEncap,
        Aigp = 26,
        LargeCommunity = 32
    }
}
