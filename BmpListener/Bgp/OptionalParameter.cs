using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public abstract class OptionalParameter
    {
        public enum Type
        {
            Capabilities = 2
        }

        protected OptionalParameter(ref ArraySegment<byte> data)
        {
            ParameterType = (Type) data.First();
            var length = data.ElementAt(1);
            data = new ArraySegment<byte>(data.Array, data.Offset + 2, length);
        }

        public Type ParameterType { get; }

        public static OptionalParameter GetOptionalParameter(ArraySegment<byte> data)
        {
            if ((Type) data.First() == Type.Capabilities)
                return new OptionalParameterCapability(data);
            return new OptionParameterUnknown(data);
        }
    }
}