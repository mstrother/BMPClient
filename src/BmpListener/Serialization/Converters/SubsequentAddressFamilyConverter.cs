using BmpListener.Bgp;
using Newtonsoft.Json;
using System;

namespace BmpListener.Serialization.Converters
{
    public class SubsequentAddressFamilyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SubsequentAddressFamily);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var safi = (SubsequentAddressFamily)value;
            switch (safi)
            {
                case SubsequentAddressFamily.Multicast:
                    writer.WriteValue("multicast");
                    break;
                case SubsequentAddressFamily.Unicast:
                    writer.WriteValue("unicast");
                    break;
            }
        }
    }
}
