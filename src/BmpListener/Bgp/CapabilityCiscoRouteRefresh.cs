using System;

namespace BmpListener.Bgp
{
    public class CapabilityCiscoRouteRefresh : Capability
    {
        public CapabilityCiscoRouteRefresh(byte[] data, int offset) 
            : base(data, offset)
        {
        }
    }
}