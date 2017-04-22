namespace BmpListener.Bgp
{
    public abstract class OptionalParameter
    {
        public OptionalParameterType Type { get; private set; }
        public int Length { get; private set; }

        public virtual void Decode(byte[] data, int offset)
        {
            Type = (OptionalParameterType)data[offset];
            Length = data[offset + 1];
        }
    }
}
