using System;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public RouteMonitoring(BmpHeader bmpHeader, ArraySegment<byte> data)
            : base(bmpHeader, ref data)
        {
            ParseBody(data);
        }

        public BgpMessage Update { get; set; }

        public void ParseBody(ArraySegment<byte> data)
        {
            Update = BgpMessage.GetBgpMessage(data);
        }
    }
}