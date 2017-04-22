using BmpListener.Bgp;
using System.Collections.Generic;
using System.Net;

namespace BmpListener.Serialization.Models
{
    public class PrefixAnnounceModel
    {
        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }
        public IPAddress Nexthop { get; set; }
        public IList<IPAddrPrefix> Prefixes { get; set; } = new List<IPAddrPrefix>();
    }
}
