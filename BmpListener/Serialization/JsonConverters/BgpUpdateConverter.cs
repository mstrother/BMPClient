using BmpListener.Bgp;
using BmpListener.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BmpListener.Serialization.JsonConverters
{
    public class BgpUpdateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BgpUpdateMessage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var updateMsg = (BgpUpdateMessage)value;
            var model = new JsonModel();

            model.Origin = updateMsg.Attributes.OfType<PathAttributeOrigin>()
                .FirstOrDefault()?.Origin;

            // TODO: multiple AS paths in message
            model.AsPath = updateMsg.Attributes.OfType<PathAttributeASPath>();

            model.Med = updateMsg.Attributes.OfType<PathAttributeMultiExitDisc>()
                .FirstOrDefault()?.Metric;

            model.AtomicAggregate = updateMsg.Attributes
                .OfType<PathAttrAtomicAggregate>().Any();

            model.Aggregator = updateMsg.Attributes
                .OfType<PathAttributeAggregator>().FirstOrDefault();

            model.Withdraw = updateMsg.Attributes
                .OfType<PathAttributeMPUnreachNLRI>().FirstOrDefault();

            //model.LargeCommunities = updateMsg.Attributes
            //    .OfType<PathAttributeLargeCommunities>().FirstOrDefault();

            model.Communities = updateMsg.Attributes
                .OfType<PathAttributeCommunity>().ToList();

            var pathAttributeMPReadNLRI = updateMsg.Attributes
                .OfType<PathAttributeMPReachNLRI>().FirstOrDefault();
            if (pathAttributeMPReadNLRI != null)
            {
                model.Announce = new AnnounceModel(pathAttributeMPReadNLRI);
            }
            else
            {
                var nexthop = updateMsg.Attributes
                    .OfType<PathAttributeNextHop>().FirstOrDefault()?.NextHop;
                model.Announce = new AnnounceModel(nexthop, updateMsg.Nlri);
            }
            
            var json = JsonConvert.SerializeObject(model);
            writer.WriteRawValue(json);
        }

        private class JsonModel
        {
            public PathAttributeOrigin.Type? Origin { get; set; }
            public IEnumerable<PathAttributeASPath> AsPath { get; set; }
            public int? Med { get; set; }
            public List<PathAttributeCommunity> Communities { get; set; }
            public PathAttributeLargeCommunities LargeCommunities { get; set; }
            public bool AtomicAggregate { get; set; }
            public PathAttributeAggregator Aggregator { get; set; }
            public AnnounceModel Announce { get; set; }
            public PathAttributeMPUnreachNLRI Withdraw { get; set; }
        }

        private class AnnounceModel
        {
            public AnnounceModel(PathAttributeMPReachNLRI nlri)
            {
                var afi = nlri.AFI.ToFriendlyString();
                var safi = nlri.SAFI.ToFriendlyString();
                Nexthop = nlri.NextHop;
                LinkLocalNextHop = nlri.LinkLocalNextHop;
                Routes = new Dictionary<string, IList<IPAddrPrefix>>
                {
                    { $"{afi} {safi}", nlri.NLRI }
                };
            }

            public AnnounceModel(IPAddress nexthop, IList<IPAddrPrefix> nlri)
            {
                var afi = AddressFamily.IP.ToFriendlyString();
                var safi = SubsequentAddressFamily.Unicast.ToFriendlyString();
                Nexthop = nexthop;
                Routes = new Dictionary<string, IList<IPAddrPrefix>>
                {
                    { $"{afi} {safi}", nlri}
                };
            }

            public IPAddress Nexthop { get; set; }
            public IPAddress LinkLocalNextHop { get; set; }
            public Dictionary<string, IList<IPAddrPrefix>> Routes { get; set; }
        }
    }
}

