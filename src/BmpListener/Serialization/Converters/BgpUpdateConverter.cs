using BmpListener.Bgp;
using BmpListener.Serialization.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BmpListener.Serialization.Converters
{
    public class BgpUpdateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BgpUpdateMessage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var bgpMsg = (BgpUpdateMessage)value;

            writer.WriteStartObject();

            var origin = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.ORIGIN) as PathAttributeOrigin;
            writer.WritePropertyName("origin");
            writer.WriteValue(origin.ToString());

            var asPathSegments = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.AS_PATH) as PathAttributeASPath;
            writer.WritePropertyName("asPath");
            writer.WriteStartArray();
            for (int i = 0; i < asPathSegments.ASPaths[0].ASNs.Count; i++)
            {
                writer.WriteValue(asPathSegments.ASPaths[0].ASNs[i]);
            }
            writer.WriteEndArray();

            //if (bgpMsg.Nlri.Count > 0)
            //{
            //    var nexthop = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NEXT_HOP) as PathAttributeNextHop;

            //    writer.WritePropertyName("announce");
            //    writer.WriteStartObject();
            //    var announceJson = SerializeAnnouncement(AddressFamily.IP, SubsequentAddressFamily.Unicast, nexthop.NextHop, bgpMsg.Nlri);
            //    writer.WriteRaw(announceJson);
            //    writer.WriteEndObject();
            //}

            //if (bgpMsg.WithdrawnRoutes.Count > 0)
            //{
            //    writer.WritePropertyName("withdraw");
            //    writer.WriteStartObject();
            //    var announceJson = SerializeWithdrawal(AddressFamily.IP, SubsequentAddressFamily.Unicast, bgpMsg.WithdrawnRoutes);
            //    writer.WriteRaw(announceJson);
            //    writer.WriteEndObject();
            //}

            writer.WriteEndObject();
        }

        //private string SerializeAnnouncement(AddressFamily afi, SubsequentAddressFamily safi, IPAddress nexthop, IList<IPAddrPrefix> prefixes)
        //{
        //    var model = new PrefixAnnouncement
        //    {
        //        Afi = "ip",
        //        Safi = "unicast",
        //        Nexthop = nexthop.ToString()
        //    };

        //    for (int i = 0; i < prefixes.Count; i++)
        //    {
        //        var prefix = prefixes[i].ToString();
        //        model.Prefixes.Add(prefix);
        //    }

        //    return JsonConvert.SerializeObject(model);
        //}

        //private string SerializeWithdrawal(AddressFamily afi, SubsequentAddressFamily safi, IList<IPAddrPrefix> prefixes)
        //{
        //    var model = new PrefixAnnouncement
        //    {
        //        Afi = "ip",
        //        Safi = "unicast"
        //    };

        //    for (int i = 0; i < prefixes.Count; i++)
        //    {
        //        var prefix = prefixes[i].ToString();
        //        model.Prefixes.Add(prefix);
        //    }

        //    return JsonConvert.SerializeObject(model);
        //}
    }
}
