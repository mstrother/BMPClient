using System;

namespace BmpListener.Bgp
{
    public class CapabilityAddPath : Capability
    {
        public CapabilityAddPath(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}