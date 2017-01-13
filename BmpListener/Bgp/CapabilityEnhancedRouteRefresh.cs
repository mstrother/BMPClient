using System;

namespace BmpListener.Bgp
{
    public class CapabilityEnhancedRouteRefresh : Capability
    {
        public CapabilityEnhancedRouteRefresh(ArraySegment<byte> data) : base(data)
        {
        }
    }
}