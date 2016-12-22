using System;

namespace BmpListener.BGP
{
    public class CapabilityEnhancedRouteRefresh : Capability
    {
        public CapabilityEnhancedRouteRefresh(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}