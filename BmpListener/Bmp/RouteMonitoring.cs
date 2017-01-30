using System;
using System.Linq;
using BmpListener.Bgp;
using Newtonsoft.Json;
using System.Dynamic;
using System.Collections.Generic;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public RouteMonitoring(BmpHeader bmpHeader, ArraySegment<byte> data)
            : base(bmpHeader, ref data)
        {
            ParseBody(data);
        }

        public BgpUpdateMessage Update { get; set; }

        public void ParseBody(ArraySegment<byte> data)
        {
            Update = (BgpUpdateMessage)BgpMessage.GetBgpMessage(data);
        }
    }
}