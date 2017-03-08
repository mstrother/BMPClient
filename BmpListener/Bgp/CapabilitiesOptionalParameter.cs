using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class CapabilitiesOptionalParameter
    {
        public CapabilitiesOptionalParameter(byte[] data, int offset, int length)
        {
            Capabilities = new List<Capability>();
            Decode(data, offset, length);
        }

        public List<Capability> Capabilities { get; }

        public void Decode(byte[] data, int offset, int length)
        {
            // check data length
            for (int i = 0; i < length;)
            {
                var capability = Capability.GetCapability(data, offset + i);
                Capabilities.Add(capability);
                i += capability.CapabilityLength + 2;
            }
        }
    }
}