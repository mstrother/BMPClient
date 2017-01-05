using System;

namespace BmpListener.Bgp
{
    public class CapabilityEnhancedRouteRefresh : Capability
    {
        public CapabilityEnhancedRouteRefresh(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}