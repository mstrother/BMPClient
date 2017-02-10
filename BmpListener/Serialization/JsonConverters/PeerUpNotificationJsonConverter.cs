using BmpListener.Bgp;
using BmpListener.Bmp;
using Newtonsoft.Json;
using System;
using System.Net;

namespace BmpListener.Serialization.JsonConverters
{
    public class PeerUpNotificationJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PeerUpNotification);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var peerUp = (PeerUpNotification)value;
            var model = new JsonModel
            {
                LocalAddress = peerUp.LocalAddress,
                LocalPort = peerUp.LocalPort,
                RemotePort = peerUp.RemotePort,
                SentOpenMessage = peerUp.SentOpenMessage,
                ReceivedOpenMessage = peerUp.ReceivedOpenMessage
            };

            var json = JsonConvert.SerializeObject(model);
            writer.WriteRawValue(json);
        }

        private class JsonModel
        {
            [JsonProperty(Order = 6)]
            public IPAddress LocalAddress { get; set; }

            [JsonProperty(Order = 7)]
            public int LocalPort { get; set; }

            [JsonProperty(Order = 8)]
            public int RemotePort { get; set; }

            [JsonProperty(Order = 9)]
            public BgpMessage SentOpenMessage { get; set; }

            [JsonProperty(Order = 10)]
            public BgpMessage ReceivedOpenMessage { get; set; }
        }
    }
}

