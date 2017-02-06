﻿using System;
using System.Linq;
using System.Net;
using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : BmpMessage
    {
        public PeerUpNotification(BmpHeader bmpHeader, ArraySegment<byte> data)
            : base(bmpHeader, ref data)
        {
            ParseBody(data);
        }

        public IPAddress LocalAddress { get; set; }
        public ushort LocalPort { get; set; }
        public ushort RemotePort { get; set; }
        public BgpMessage SentOpenMessage { get; set; }
        public BgpMessage ReceivedOpenMessage { get; set; }
        
        public void ParseBody(ArraySegment<byte> data)
        {
            //if ((message.PeerHeader.Flags & (1 << 7)) != 0)
            //{
            LocalAddress = new IPAddress(data.Take(16).ToArray());
            //}

            LocalPort = data.ToUInt16(16);
            RemotePort = data.ToUInt16(18);

            var offset = data.Offset + 20;
            var count = data.Count - 20;
            data = new ArraySegment<byte>(data.Array, offset, count);

            SentOpenMessage = BgpMessage.GetBgpMessage(data);

            offset = data.Offset + SentOpenMessage.Header.Length;
            count = data.Count - SentOpenMessage.Header.Length;
            data = new ArraySegment<byte>(data.Array, offset, count);

            ReceivedOpenMessage = BgpMessage.GetBgpMessage(data);
        }
    }
}