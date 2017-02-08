using BmpListener.Bmp;
using BmpListener.Serialization.JsonConverters;
using BmpListener.Serialization.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BmpListener.Serialization
{
    public static class BmpSerializer
    {
        static BmpSerializer()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                //settings.ContractResolver = new ConverterContractResolver();
                settings.Converters.Add(new PathAttributeJsonConverter());
                settings.Converters.Add(new IPAddressJsonConverter());
                settings.Converters.Add(new IPAddrPrefixJsonConverter());
                settings.Converters.Add(new BmpPeerHeaderJsonConverter());
                settings.Converters.Add(new BgpUpdateConverter());
                settings.Converters.Add(new BgpOpenMessageJsonConverter());
                settings.Converters.Add(new StringEnumConverter(true));
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                return settings;
            };
        }

        public static string ToJson(BmpMessage message)
        {
            var jsonMsg = JsonMessage.Create(message);
            var json = JsonConvert.SerializeObject(jsonMsg);
            return json;
        }
    }
}
