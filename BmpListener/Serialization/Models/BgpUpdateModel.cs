using BmpListener.Bgp;
using BmpListener.Bmp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BmpListener.Serialization.Models
{
    public class BgpUpdateModel
    {
        public BgpUpdateModel(BgpUpdateMessage updateMsg)
        {
            Origin = updateMsg.Attributes.OfType<PathAttributeOrigin>()
            .FirstOrDefault()?.Origin;

            AsPath = updateMsg.Attributes.OfType<PathAttributeASPath>()
            .FirstOrDefault()?.ASPaths.FirstOrDefault()?.ASNs;

            Med = updateMsg.Attributes.OfType<PathAttributeMultiExitDisc>()
            .FirstOrDefault()?.Metric;

            AtomicAggregate = updateMsg.Attributes
                .OfType<PathAttrAtomicAggregate>().Any();

            Aggregator = updateMsg.Attributes
                .OfType<PathAttributeAggregator>().FirstOrDefault();

            Announce = updateMsg.Attributes
               .OfType<PathAttributeMPReachNLRI>().FirstOrDefault();

            Withdraw = updateMsg.Attributes
                .OfType<PathAttributeMPUnreachNLRI>().FirstOrDefault();
        }

        public BgpUpdateModel(RouteMonitoring message)
            : this((BgpUpdateMessage)message.BgpMessage)
        { }

        public Origin? Origin { get; set; }
        public int[] AsPath { get; set; }
        public int? Med { get; set; }
        public bool AtomicAggregate { get; set; }
        public PathAttributeAggregator Aggregator { get; set; }
        public PathAttributeMPReachNLRI Announce { get; set; }
        public PathAttributeMPUnreachNLRI Withdraw { get; set; }

        public class AnnounceModel
        {
            public IPAddress Nexthop { get; set; }
            public IPAddress LinkLocalNextHop { get; set; }
            public Dictionary<string, IPAddrPrefix[]> Routes { get; set; }
        }
    }
}
