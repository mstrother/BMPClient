using System;

namespace BmpListener.Bgp
{
    public class CapabilityRouteRefresh : Capability
    {
        public CapabilityRouteRefresh(CapabilityCode capability, ArraySegment<byte> bytes)
            : base(capability, bytes)
        {
        }
    }
}