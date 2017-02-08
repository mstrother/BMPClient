using BmpListener.Bgp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;

namespace BmpListener.Serialization.JsonConverters
{
    public class PathAttributeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(PathAttribute).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string json;
            var pathAttribute = (PathAttribute)value;
            switch (pathAttribute.AttributeType)
            {
                case (PathAttributeType.AGGREGATOR):
                    json = Serialize((PathAttributeAggregator)pathAttribute);
                    break;
                case (PathAttributeType.MP_REACH_NLRI):
                    json = Serialize((PathAttributeMPReachNLRI)pathAttribute);
                    break;
                case (PathAttributeType.MP_UNREACH_NLRI):
                    json = Serialize((PathAttributeMPUnreachNLRI)pathAttribute);
                    break;
                case (PathAttributeType.LARGE_COMMUNITY):
                    json = Serialize((PathAttributeLargeCommunities)pathAttribute);
                    break;
                default:
                    json = JsonConvert.SerializeObject(pathAttribute);
                    break;
            }
            writer.WriteRawValue(json);
        }

        public string Serialize(PathAttributeAggregator pathAttribute)
        {
            dynamic model = new ExpandoObject();
            model.asn = pathAttribute.AS;
            model.ip = pathAttribute.IPAddress;
            var json = JsonConvert.SerializeObject(model);
            return json;
        }

        public string Serialize(PathAttributeMPReachNLRI pathAttribute)
        {
            var afi = pathAttribute.AFI.ToFriendlyString();
            var safi = pathAttribute.SAFI.ToFriendlyString();
            var model = new AnnounceModel
            {
                Nexthop = pathAttribute.NextHop,
                LinkLocalNextHop = pathAttribute.LinkLocalNextHop,
                Routes = new Dictionary<string, IPAddrPrefix[]>
                    {
                        { $"{afi} {safi}", pathAttribute.Value }
                    }
            };
            var json = JsonConvert.SerializeObject(model);
            return json;

        }

        public string Serialize(PathAttributeMPUnreachNLRI pathAttribute)
        {
            var routes = pathAttribute.Value;
            var model = new Dictionary<string, IPAddrPrefix[]>();
            var afi = pathAttribute.AFI.ToFriendlyString();
            var safi = pathAttribute.SAFI.ToFriendlyString();
            model.Add($"{afi} {safi}", routes);
            var json = JsonConvert.SerializeObject(model);
            return json;
        }

        public string Serialize(PathAttributeLargeCommunities pathAttribute)
        {
            var canonicalForm = pathAttribute.ToString();
            var json = JsonConvert.SerializeObject(canonicalForm);
            return json;
        }
        
        private class AnnounceModel
        {
            public IPAddress Nexthop { get; set; }
            public IPAddress LinkLocalNextHop { get; set; }
            public Dictionary<string, IPAddrPrefix[]> Routes { get; set; }
        }
    }
}
