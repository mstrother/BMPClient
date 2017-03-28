using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class CapabilitiesOptionalParameter : OptionalParameter
    {
        public CapabilitiesOptionalParameter(byte[] data, int offset)
            : base(data, offset)
        {
            Capabilities = new List<Capability>();
            Decode(data, offset + 2);
        }

        public List<Capability> Capabilities { get; }

        public void Decode(byte[] data, int offset)
        {
            // check data length
            for (int i = 0; i < ParameterLength;)
            {
                var capability = Capability.GetCapability(data, offset + i);
                Capabilities.Add(capability);
                i += capability.CapabilityLength + 2;
            }
        }
    }
}