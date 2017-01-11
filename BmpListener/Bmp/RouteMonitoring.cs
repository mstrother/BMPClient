using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : IBMPBody
    {
        public RouteMonitoring(ArraySegment<byte> data)
        {
            ParseBody(data);
        }

        public BgpMessage BGPUpdate { get; set; }

        public void ParseBody(ArraySegment<byte> data)
        {
            BGPUpdate = BgpMessage.GetBgpMessage(data);
        }
    }
}