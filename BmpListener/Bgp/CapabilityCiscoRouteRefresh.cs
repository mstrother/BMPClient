using System;

namespace BmpListener.Bgp
{
    public class CapabilityCiscoRouteRefresh : Capability
    {
        public CapabilityCiscoRouteRefresh(ArraySegment<byte> data) : base(data)
        {
        }
    }
}