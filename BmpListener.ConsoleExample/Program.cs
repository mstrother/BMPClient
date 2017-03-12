using System;
using BmpListener.Serialization;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        private static void Main()
        {
            var bmpListener = new BmpListener();
            bmpListener.OnMessageReceived += WriteJson;
            bmpListener.Start().Wait();
        }

        private static void WriteJson(object sender, MessageReceivedEventArgs e)
        {
            var json = BmpJsonSerializer.Serialize(e.BmpMessage);
            Console.WriteLine(json);
            return;
        }
    }
}