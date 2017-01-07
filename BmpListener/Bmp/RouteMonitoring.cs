using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : IBMPBody
    {
        public RouteMonitoring(BmpMessage message, byte[] data)
        {
            ParseBody(message, data);
        }

        public BgpMessage BGPUpdate { get; set; }

        public void ParseBody(BmpMessage message, byte[] data)
        {
            var dataSegment = new ArraySegment<byte>(data);
            BGPUpdate = BgpMessage.GetBgpMessage(dataSegment);
        }
    }
}