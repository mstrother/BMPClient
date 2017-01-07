using System;

namespace BmpListener.Bgp
{
    public class CapabilityCiscoRouteRefresh : Capability
    {
        public CapabilityCiscoRouteRefresh(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}