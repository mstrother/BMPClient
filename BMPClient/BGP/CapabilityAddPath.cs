using System;

namespace BMPClient.BGP
{
    public class CapabilityAddPath : Capability
    {
        public CapabilityAddPath(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}