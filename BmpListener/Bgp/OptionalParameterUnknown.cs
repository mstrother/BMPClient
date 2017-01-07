using System;

namespace BmpListener.Bgp
{
    public class OptionParameterUnknown : OptionalParameter
    {
        public OptionParameterUnknown(ArraySegment<byte> data) : base(data)
        {
        }
    }
}