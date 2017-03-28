using System;

namespace BmpListener.Bgp
{
    public class CapabilityGracefulRestart : Capability
    {
        public CapabilityGracefulRestart(byte[] data, int offset)
            : base(data, offset)
        {
        }
    }
}