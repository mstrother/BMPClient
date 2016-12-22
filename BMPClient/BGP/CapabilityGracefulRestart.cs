using System;

namespace BmpListener.BGP
{
    public class CapabilityGracefulRestart : Capability
    {
        public CapabilityGracefulRestart(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}