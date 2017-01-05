using System;
using System.Net;
using BmpListener.Bmp;
using BmpListener.JSON;
using Newtonsoft.Json;

namespace BmpListener
{
    internal class Program
    {
        private static void Main()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new IPAddressConverter());
                settings.Converters.Add(new TestConverter());
                return settings;
            };

            var ip = IPAddress.Parse("192.168.1.126");
            var bmpListener = new BmpListener(ip);
            bmpListener.Start(WriteJson).Wait();
        }

        private static void WriteJson(BmpMessage msg)
        {
            var json = JsonConvert.SerializeObject(msg);
            Console.WriteLine(json);
        }
    }
}