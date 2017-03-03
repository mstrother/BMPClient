﻿using System;
using BmpListener.Bgp;
namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public RouteMonitoring(BmpHeader bmpHeader, ArraySegment<byte> data)
            : base(bmpHeader, data)
        {
            ParseBody(MessageData);
        }

        public BgpMessage BgpMessage { get; set; }
        
        public void ParseBody(ArraySegment<byte> data)
        {
            BgpMessage = (BgpUpdateMessage)BgpMessage.GetBgpMessage(data);
        }
    }
}