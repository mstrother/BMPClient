using BmpListener.Extensions;
using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class CapabilityMultiProtocol : Capability
    {
        public CapabilityMultiProtocol(ArraySegment<byte> data) : base(data)
        {
            var afi = data.ToUInt16(2);
            var safi = data.ElementAt(4);
        }
    }
}