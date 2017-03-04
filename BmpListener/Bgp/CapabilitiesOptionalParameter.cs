using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class CapabilitiesOptionalParameter
    {
        public CapabilitiesOptionalParameter(ArraySegment<byte> data)
        {
            Capabilities = new List<Capability>();
            DecodeFromBytes(data);
        }

        public List<Capability> Capabilities { get; }

        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            var count = data.Count;
            while (data.Count > 0)
            {
                var capability = Capability.GetCapability(data);
                Capabilities.Add(capability);
                var length = capability.CapabilityLength + 2;
                offset += length;
                count -= length;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }
    }
}