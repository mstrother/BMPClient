using System;

namespace BmpListener.Bgp
{
    public class CapabilityGracefulRestart : Capability
    {
        public CapabilityGracefulRestart(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}