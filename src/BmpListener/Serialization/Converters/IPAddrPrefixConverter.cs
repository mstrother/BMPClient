using BmpListener.Bgp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BmpListener.Serialization.Converters
{
    class IPAddrPrefixConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddrPrefix));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
