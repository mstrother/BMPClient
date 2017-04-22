using BmpListener.Bgp;
using System.Collections.Generic;

namespace BmpListener.Serialization.Models
{
    public class PrefixWithdrawal
    {
        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }
        public IList<IPAddrPrefix> Prefixes { get; set; } = new List<IPAddrPrefix>();
    }
}
