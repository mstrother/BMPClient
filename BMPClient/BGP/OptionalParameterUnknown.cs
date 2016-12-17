using System;

namespace BMPClient.BGP
{
    public class OptionParameterUnknown : OptionalParameter
    {
        public OptionParameterUnknown(ArraySegment<byte> data) : base(data)
        {
        }
    }
}