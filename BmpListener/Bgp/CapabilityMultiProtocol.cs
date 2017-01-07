using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class CapabilityMultiProtocol : Capability
    {
        public CapabilityMultiProtocol(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
            var afi = data.ToUInt16(0);
            var safi = data.ElementAt(3);
        }
    }
}