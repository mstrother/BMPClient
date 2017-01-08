﻿using System;
using BmpListener.Bmp;
using BmpListener.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
                settings.Converters.Add(new TestConverter());
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            };

            var bmpListener = new BmpListener();
            bmpListener.Start(WriteJson).Wait();
        }

        private static void WriteJson(BmpMessage msg)
        {
            var json = JsonConvert.SerializeObject(msg);
            Console.WriteLine(json);
        }
    }
}