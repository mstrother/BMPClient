using BmpListener.Bgp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BmpListener.Serialization.Converters
{
    public class BgpOpenConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BgpOpenMessage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var bgpOpenMsg = (BgpOpenMessage)value;

            writer.WriteStartObject();

            writer.WritePropertyName("asn");
            writer.WriteValue(bgpOpenMsg.MyAS);
            writer.WritePropertyName("holdTime");
            writer.WriteValue(bgpOpenMsg.HoldTime);
            writer.WritePropertyName("id");
            writer.WriteValue(bgpOpenMsg.BgpIdentifier.ToString());

            writer.WritePropertyName("capability");
            writer.WriteStartObject();

            var capabilities = bgpOpenMsg.OptionalParameters.FirstOrDefault(x => x.Type == OptionalParameterType.Capability)
                as CapabilitiesParameter;

            foreach (var capability in capabilities.Capabilities)
            {
                switch (capability.Code)
                {
                    case CapabilityCode.Multiprotocol:
                        //jsonObject = ConvertToJson((CapabilityMultiProtocol)value);
                        break;
                    case CapabilityCode.RouteRefresh:
                        writer.WritePropertyName("routeRefresh");
                        writer.WriteValue(true);
                        break;
                    case CapabilityCode.GracefulRestart:
                        //jsonObject = ConvertToJson((CapabilityGracefulRestart)value);
                        break;
                    case CapabilityCode.FourOctetAs:
                        writer.WritePropertyName("fourOctectAsn");
                        writer.WriteValue(((CapabilityFourOctetAsNumber)capability).Asn);
                        break;
                    case CapabilityCode.AddPath:
                        //jsonObject = ConvertToJson((CapabilityAddPath)value);
                        break;
                    case CapabilityCode.EnhancedRouteRefresh:
                        writer.WritePropertyName("enhancedRouteRefresh");
                        writer.WriteValue(true);
                        break;
                    case CapabilityCode.CiscoRouteRefresh:
                        writer.WritePropertyName("ciscoRouteRefresh");
                        writer.WriteValue(true); ;
                        break;
                }
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
