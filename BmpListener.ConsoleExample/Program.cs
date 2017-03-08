using System;
using BmpListener.Bmp;
using BmpListener.Serialization;

namespace BmpListener.ConsoleExample
{
    internal class Program
    {
        private static void Main()
        {
            var bmpListener = new BmpListener();
            bmpListener.Start(WriteJson).Wait();
        }

        private static void WriteJson(BmpMessage msg)
        {
            return;
            //var json = BmpSerializer.ToJson(msg);
            //Console.WriteLine(json);
        }
    }
}