using BmpListener.Bgp;
using System;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public BgpMessage BgpMessage { get; set; }

        public override void Decode(ArraySegment<byte> data)
        {
            BgpMessage = BgpMessage.DecodeMessage(data);
        }
    }
}