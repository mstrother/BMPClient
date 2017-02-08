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
                As = openMsg.MyAS,
                HoldTime = openMsg.HoldTime,
                Id = openMsg.Id,
                Capabilities = new List<Capability>()
            };

            foreach (var capability in openMsg.Capabilities)
            {
                if (capability.CapabilityType != CapabilityCode.FourOctetAs)
                {
                    model.Capabilities.Add(new Capability
                    {
                        Type = capability.CapabilityType
                    });
                }
                else
                {
                    model.Capabilities.Add(new FourOctetAsCapability
                    {
                        Type = capability.CapabilityType,
                        As = ((CapabilityFourOctetAsNumber)capability).CapabilityValue
                    });
                }
            }

            var json = JsonConvert.SerializeObject(model);
            writer.WriteRawValue(json);
        }

        private class JsonModel
        {
            public byte Version { get; set; }
            public ushort As { get; set; }
            public ushort HoldTime { get; set; }
            public IPAddress Id { get; set; }
            public List<Capability> Capabilities { get; set; }
        }

        private class Capability
        {
            [JsonProperty(Order = 1)]
            public CapabilityCode Type { get; set; }
        }

        private class FourOctetAsCapability : Capability
        {
            [JsonProperty(Order = 2)]
            public uint As { get; set; }
        }
    }
}
