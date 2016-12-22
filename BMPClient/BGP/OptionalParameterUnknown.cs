using System;

namespace BmpListener.BGP
{
    public class OptionParameterUnknown : OptionalParameter
    {
        public OptionParameterUnknown(ArraySegment<byte> data) : base(data)
        {
        }
    }
}