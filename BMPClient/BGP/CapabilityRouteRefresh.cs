using System;

namespace BmpListener.BGP
{
    public class CapabilityRouteRefresh : Capability
    {
        public CapabilityRouteRefresh(CapabilityCode capability, ArraySegment<byte> bytes)
            : base(capability, bytes)
        {
        }
    }
}