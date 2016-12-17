using System;
using BMPClient.BGP;
using Newtonsoft.Json;

namespace BMPClient.JSON
{
    public class TestConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPAddrPrefix);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(IPAddrPrefix))
            {
                var addrPrefix = (IPAddrPrefix) value;
                writer.WriteValue($"{addrPrefix.Prefix}/{addrPrefix.Length}");
            }
        }
    }
}