using System;

namespace BmpListener.Bgp
{
    public class CapabilityGracefulRestart : Capability
    {
        public CapabilityGracefulRestart(ArraySegment<byte> data): base(data)
        {
        }
    }
}