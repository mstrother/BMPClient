using BmpListener.Bgp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace BmpListener.Serialization.JsonConverters
{
    class BgpOpenMessageJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BgpOpenMessage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var openMsg = (BgpOpenMessage)value;

            var model = new JsonModel
            {
                Version = openMsg.Version,
                Asn = openMsg.MyAS,
                HoldTime = openMsg.HoldTime,
                Id = openMsg.Id,
                Capabilities = openMsg.Capabilities
            };

            var json = JsonConvert.SerializeObject(model);
            writer.WriteRawValue(json);
        }

        private class JsonModel
        {
            public byte Version { get; set; }
            public int Asn { get; set; }
            public int HoldTime { get; set; }
            public IPAddress Id { get; set; }
            public IList<Bgp.Capability> Capabilities { get; set; }
        }

        private class Capability
        {
            [JsonProperty(Order = 1)]
            public CapabilityCode Type { get; set; }
        }

        private class FourOctetAsCapability : Capability
        {
            [JsonProperty(Order = 2)]
            public int Asn { get; set; }
        }
    }
}
