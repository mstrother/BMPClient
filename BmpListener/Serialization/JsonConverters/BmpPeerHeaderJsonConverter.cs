using BmpListener.Bmp;
using Newtonsoft.Json;
using System;
using System.Net;

namespace BmpListener.Serialization.JsonConverters
{
    public class BmpPeerHeaderJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PerPeerHeader);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var peerHeader = (PerPeerHeader)value;
            var model = new JsonModel
            {
                Asn = peerHeader.AS,
                Ip = peerHeader.PeerAddress,
                Id = peerHeader.PeerBGPId,
                Distinguisher = peerHeader.PeerDistinguisher,
                Type = peerHeader.PeerType,
                PostPolicy = peerHeader.IsPostPolicy
            };

            var json = JsonConvert.SerializeObject(model);
            writer.WriteRawValue(json);
        }

        private class JsonModel
        {
            public int Asn { get; set; }
            public IPAddress Ip { get; set; }
            public IPAddress Id { get; set; }
            public ulong Distinguisher { get; set; }
            public PerPeerHeader.Type Type { get; set; }
            public bool PostPolicy { get; set; }
        }
    }
}
