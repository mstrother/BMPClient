using System;

namespace BmpListener.BGP
{
    public class CapabilityAddPath : Capability
    {
        public CapabilityAddPath(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}