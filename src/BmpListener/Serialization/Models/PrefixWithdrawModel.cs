using BmpListener.Bgp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BmpListener.Serialization.Models
{
    public class PrefixWithdrawal
    {
        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }
        public IList<IPAddrPrefix> Prefixes { get; set; } = new List<IPAddrPrefix>();
    }
}
