using BmpListener.Bgp;

namespace BmpListener.Serialization.Models
{
    public class BgpOpenModel
    {
        public int Asn { get; set; }
        public int HoldTime { get; set; }
        public string Id { get; set; }
        public int? FourOctectAsn { get; set; }
        public AddressFamily? Afi { get; set; }
        public SubsequentAddressFamily? Safi { get; set; }
    }
}
