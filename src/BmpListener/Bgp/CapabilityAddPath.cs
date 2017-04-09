namespace BmpListener.Bgp
{
    public class CapabilityAddPath : Capability
    {    
        public AddressFamily Afi { get; private set; }
        public SubsequentAddressFamily Safi { get; private set; }
    }
}