using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class CapabilitiesParameter : OptionalParameter
    {
        public IList<Capability> Capabilities { get; } = new List<Capability>();

        public override void Decode(ArraySegment<byte> data)
        {
            base.Decode(data);
            data = new ArraySegment<byte>(data.Array, data.Offset + 2, Length);

            for (var i = 0; i < Length;)
            {
                (Capability capability, int length) = Capability.DecodeCapability(data);
                Capabilities.Add(capability);
                data = new ArraySegment<byte>(data.Array, data.Offset + length, data.Count - length);
                i += length;
            }
        }
    }
}