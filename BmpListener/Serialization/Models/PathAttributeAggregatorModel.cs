using BmpListener.Bgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BmpListener.Serialization.Models
{
    public class PathAttributeAggregatorModel
    {
        public PathAttributeAggregatorModel(PathAttributeAggregator aggregator)
        {
            Asn = aggregator.AS;
            Ip = aggregator.IPAddress;
        }

        public int Asn { get; set; }
        public IPAddress Ip { get; set; }
    }
}
