using BmpListener.Bgp;
using Newtonsoft.Json;
using System;

namespace BmpListener.Serialization.Converters
{
    public class AddressFamilyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AddressFamily);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var afi = (AddressFamily)value;
            switch (afi)
            {
                case AddressFamily.IP:
                    writer.WriteValue("IP");
                    break;
                case AddressFamily.IP6:
                    writer.WriteValue("IPv6");
                    break;
                case AddressFamily.L2VPN:
                    writer.WriteValue("L2VPN");
                    break;
            }
        }
    }
}
