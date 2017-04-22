namespace BmpListener.Bgp
{
    // http://www.iana.org/assignments/capability-codes/capability-codes.xhtml
    public enum CapabilityCode
    {
        Multiprotocol = 1,
        RouteRefresh = 2,
        //TODO capability code 4
        GracefulRestart = 64,
        FourOctetAs = 65,
        AddPath = 69,
        EnhancedRouteRefresh = 70,
        CiscoRouteRefresh = 128
        //TODO capability code 129
    }
}
