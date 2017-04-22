using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class CapabilitiesParameter : OptionalParameter
    {
        public IList<Capability> Capabilities { get; } = new List<Capability>();

        public override void Decode(byte[] data, int offset)
        {
            base.Decode(data, offset);
            offset += 2;

            for (var i = 0; i < Length;)
            {
                (Capability capability, int length) = Capability.DecodeCapability(data, offset);
                Capabilities.Add(capability);
                offset += length;
                i += length;
            }
        }
    }
}