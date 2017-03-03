using BmpListener.Bgp;
using BmpListener.Extensions;
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
            switch (((PathAttribute)value).AttributeType)
            {
                case (PathAttributeType.AS_PATH):
                    json = Serialize((PathAttributeASPath)value);
                    break;
                case (PathAttributeType.AGGREGATOR):
                    json = Serialize((PathAttributeAggregator)value);
                    break;
                case (PathAttributeType.COMMUNITY):
                    json = Serialize((PathAttributeCommunity)value);
                    break;
                case (PathAttributeType.LARGE_COMMUNITY):
                    json = Serialize((PathAttributeCommunity)value);
                    break;
                case (PathAttributeType.MP_REACH_NLRI):
                    json = Serialize((PathAttributeMPReachNLRI)value);
                    break;
                case (PathAttributeType.MP_UNREACH_NLRI):
                    json = Serialize((PathAttributeMPUnreachNLRI)value);
                    break;
                default:
                    json = JsonConvert.SerializeObject(value);
                    break;
            }
            writer.WriteRawValue(json);
        }

        public string Serialize(PathAttributeASPath pathAttribute)
        {
            //dynamic model = new ExpandoObject();
            //model.asn = pathAttribute.AS;
            //model.ip = pathAttribute.IPAddress;
            var json = JsonConvert.SerializeObject(pathAttribute.ASPaths);
            return json;
        }

        public string Serialize(PathAttributeAggregator pathAttribute)
        {
            dynamic model = new ExpandoObject();
            model.asn = pathAttribute.AS;
            model.ip = pathAttribute.IPAddress;
            var json = JsonConvert.SerializeObject(model);
            return json;
        }

        public string Serialize(PathAttributeCommunity pathAttribute)
        {
            var model = new CommunityModel(pathAttribute.Community);
            var json = JsonConvert.SerializeObject(model);
            return json;
        }

        public string Serialize(PathAttributeLargeCommunities pathAttribute)
        {
            var canonicalForm = pathAttribute.ToString();
            var json = JsonConvert.SerializeObject(canonicalForm);
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
                Routes = new Dictionary<string, IList<IPAddrPrefix>>
                    {
                        { $"{afi} {safi}", pathAttribute.NLRI }
                    }
            };
            var json = JsonConvert.SerializeObject(model);
            return json;

        }

        public string Serialize(PathAttributeMPUnreachNLRI pathAttribute)
        {
            var routes = pathAttribute.WithdrawnRoutes;
            var model = new Dictionary<string, IList<IPAddrPrefix>>();
            var afi = pathAttribute.AFI.ToFriendlyString();
            var safi = pathAttribute.SAFI.ToFriendlyString();
            model.Add($"{afi} {safi}", routes);
            var json = JsonConvert.SerializeObject(model);
            return json;
        }

        private class AnnounceModel
        {
            public IPAddress Nexthop { get; set; }
            public IPAddress LinkLocalNextHop { get; set; }
            public Dictionary<string, IList<IPAddrPrefix>> Routes { get; set; }
        }

        private class CommunityModel
        {
            public CommunityModel(uint community)
            {
                byte[] bytes = BitConverter.GetBytes(community);
                Community = BitConverter.ToInt16(bytes, 0);
                Asn = BitConverter.ToUInt16(bytes, 2);
            }

            public int Asn { get; }
            public int Community { get; }
        }
    }
}
