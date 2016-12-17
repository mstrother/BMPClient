using System;

namespace BMPClient.BGP
{
    public class CapabilityEnhancedRouteRefresh : Capability
    {
        public CapabilityEnhancedRouteRefresh(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}