using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : IBMPBody
    {
        public RouteMonitoring(BmpMessage message, ArraySegment<byte> data)
        {
            ParseBody(message, data);
        }

        public BgpMessage BGPUpdate { get; set; }

        public void ParseBody(BmpMessage message, ArraySegment<byte> data)
        {
            BGPUpdate = BgpMessage.GetBgpMessage(data);
        }
    }
}