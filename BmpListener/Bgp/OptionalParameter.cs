
namespace BmpListener.Bgp
{
    public abstract class OptionalParameter
    {
        public OptionalParameter(byte[] data, int offset)
        {
            ParameterType = data[offset];
            ParameterLength = data[offset + 1];
        }

        public int ParameterType { get; }
        public int ParameterLength { get; }
    }
}
