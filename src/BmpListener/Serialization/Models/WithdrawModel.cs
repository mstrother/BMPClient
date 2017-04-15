using System.Collections.Generic;

namespace BmpListener.Serialization.Models
{
    public class WithdrawModel
    {
        public string Afi { get; set; }
        public string Safi { get; set; }
        public string Nexthop { get; set; }
        public IList<string> Prefixes { get; set; }
    }
}
