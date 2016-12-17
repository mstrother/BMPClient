using System;
using System.Linq;

namespace BMPClient.BGP
{
    public abstract class OptionalParameter
    {
        public enum Type : byte
        {
            Capabilities = 2
        }

        public OptionalParameter(ArraySegment<byte> data)
        {
            ParameterType = (Type) data.First();
            ParameterLength = data.ElementAt(1);
        }

        public Type ParameterType { get; }
        public byte ParameterLength { get; }

        public static OptionalParameter GetOptionalParameter(ArraySegment<byte> data)
        {
            if ((Type) data.First() == Type.Capabilities)
                return new OptionalParameterCapability(data);
            return new OptionParameterUnknown(data);
        }
    }
}