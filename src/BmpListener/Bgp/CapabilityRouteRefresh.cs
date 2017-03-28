using System;

namespace BmpListener.Bgp
{
    public class CapabilityRouteRefresh : Capability
    {
        public CapabilityRouteRefresh(byte[] bytes, int offset)
            : base(bytes, offset)
        {
        }
    }
}