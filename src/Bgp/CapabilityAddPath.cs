using System;

namespace BmpListener.Bgp
{
    public class CapabilityAddPath : Capability
    {
        public CapabilityAddPath(byte[] data, int offset) 
            : base(data, offset)
        {
        }
    }
}