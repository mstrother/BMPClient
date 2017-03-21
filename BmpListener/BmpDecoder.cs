using DotNetty.Codecs;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using BmpListener.Bmp;
using System;

namespace BmpListener
{
    public class BmpDecoder : ByteToMessageDecoder
    {
        public static BmpMessage Decode(byte[] data)
        {
            var bmpHeader = new BmpHeader(data);

            switch (bmpHeader.MessageType)
            {
                case BmpMessageType.RouteMonitoring:
                    return new RouteMonitoring(bmpHeader, data);
                case BmpMessageType.StatisticsReport:
                    return new StatisticsReport(bmpHeader, data);
                case BmpMessageType.PeerDown:
                    return new PeerDownNotification(bmpHeader, data);
                case BmpMessageType.PeerUp:
                    return new PeerUpNotification(bmpHeader, data);
                case BmpMessageType.Initiation:
                    return new BmpInitiation(bmpHeader);
                case BmpMessageType.Termination:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            try
            {
                var length = input.ReadableBytes;

                if (length < 6)
                {
                    return;
                }

                var data = new byte[length];
                input.ReadBytes(data);
                var bmpMessage = Decode(data);

                if (bmpMessage != null)
                {
                    output.Add(bmpMessage);
                }
            }
            catch (DecoderException)
            {
                input.SkipBytes(input.ReadableBytes);
                throw;
            }
        }
    }
}
