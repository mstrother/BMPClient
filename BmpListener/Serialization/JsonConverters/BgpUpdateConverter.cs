using BmpListener.Bgp;
using Newtonsoft.Json;
using System;
using System.Linq;

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

            model.AsPath = updateMsg.Attributes.OfType<PathAttributeASPath>()
                .FirstOrDefault()?.ASPaths.FirstOrDefault()?.ASNs;

            model.Med = updateMsg.Attributes.OfType<PathAttributeMultiExitDisc>()
                .FirstOrDefault()?.Metric;

            model.AtomicAggregate = updateMsg.Attributes
                .OfType<PathAttrAtomicAggregate>().Any();

            model.Aggregator = updateMsg.Attributes
                .OfType<PathAttributeAggregator>().FirstOrDefault();

            model.Announce = updateMsg.Attributes
               .OfType<PathAttributeMPReachNLRI>().FirstOrDefault();

            model.Withdraw = updateMsg.Attributes
                .OfType<PathAttributeMPUnreachNLRI>().FirstOrDefault();

            model.LargeCommunities = updateMsg.Attributes
                .OfType<PathAttributeLargeCommunities>().FirstOrDefault();

            var json = JsonConvert.SerializeObject(model);
            writer.WriteRawValue(json);
        }

        private class JsonModel
        {
            public PathAttributeOrigin.Type? Origin { get; set; }
            public int[] AsPath { get; set; }
            public int? Med { get; set; }
            public PathAttributeLargeCommunities LargeCommunities { get; set; }
            public bool AtomicAggregate { get; set; }
            public PathAttributeAggregator Aggregator { get; set; }
            public PathAttributeMPReachNLRI Announce { get; set; }
            public PathAttributeMPUnreachNLRI Withdraw { get; set; }
        }
    }
}

