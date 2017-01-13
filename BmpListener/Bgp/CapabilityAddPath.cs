using System;

namespace BmpListener.Bgp
{
    public class CapabilityAddPath : Capability
    {
        public CapabilityAddPath(ArraySegment<byte> data) : base(data)
        {
        }
    }
}