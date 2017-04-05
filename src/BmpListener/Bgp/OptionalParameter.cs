
using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public abstract class OptionalParameter
    {
        public OptionalParameterType Type { get; private set; }
        public int Length { get; private set; }

        public virtual void Decode(ArraySegment<byte> data)
        {
            Type = (OptionalParameterType)data.First();
            Length = data.ElementAt(1);
        }
    }
}
