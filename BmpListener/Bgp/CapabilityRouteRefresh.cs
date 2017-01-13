using System;

namespace BmpListener.Bgp
{
    public class CapabilityRouteRefresh : Capability
    {
        public CapabilityRouteRefresh(ArraySegment<byte> bytes): base(bytes)
        {
        }
    }
}