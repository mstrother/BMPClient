using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BmpListener.Serialization.Converters
{
    public class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }
        
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            IPAddress ip = (IPAddress)value;
            writer.WriteValue(ip.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return IPAddress.Parse(token.Value<string>());
        }
    }
}
