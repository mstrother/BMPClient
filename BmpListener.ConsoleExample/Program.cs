using System;
using BmpListener.Bmp;
using BmpListener.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        private static void Main()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new IPAddressConverter());
                settings.Converters.Add(new StringEnumConverter(true));
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                return settings;
            };

            var bmpListener = new BmpListener();
            bmpListener.Start(WriteJson).Wait();
        }

        private static void WriteJson(BmpMessage msg)
        {
            var json = msg.ToJson();
            Console.WriteLine(json);
        }
    }
}