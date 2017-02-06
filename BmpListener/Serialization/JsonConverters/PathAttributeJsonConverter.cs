using BmpListener.Bgp;
using BmpListener.Serialization.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
            string json = string.Empty;
            var pathAttribute = (PathAttribute)value;
            switch (pathAttribute.Type)
            {
                case (AttributeType.AGGREGATOR):
                    json = Serialize((PathAttributeAggregator)pathAttribute);
                    break;
                case (AttributeType.MP_REACH_NLRI):
                    json = Serialize((PathAttributeMPReachNLRI)pathAttribute);
                    break;
                case (AttributeType.MP_UNREACH_NLRI):
                    json = Serialize((PathAttributeMPUnreachNLRI)pathAttribute);
                    break;
                case (AttributeType.LARGE_COMMUNITY):
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
            var model = new PathAttributeAggregatorModel(pathAttribute);
            var json = JsonConvert.SerializeObject(model);
            return json;
        }

        public string Serialize(PathAttributeMPReachNLRI pathAttribute)
        {
            var afi = pathAttribute.AFI.ToString().ToLower();
            var safi = pathAttribute.SAFI.ToString().ToLower();
            var model = new BgpUpdateModel.AnnounceModel
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
            var afi = pathAttribute.AFI.ToString().ToLower();
            var safi = pathAttribute.SAFI.ToString().ToLower();
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
    }
}
