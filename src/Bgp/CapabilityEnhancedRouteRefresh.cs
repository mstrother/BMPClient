using System;

namespace BmpListener.Bgp
{
    public class CapabilityEnhancedRouteRefresh : Capability
    {
        public CapabilityEnhancedRouteRefresh(byte[] data, int offset) 
            : base(data, offset)
        {
        }
    }
}