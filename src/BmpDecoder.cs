using DotNetty.Codecs;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using BmpListener.Bmp;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive;

namespace BmpListener
{
    public class BmpDecoder : ByteToMessageDecoder
    {
        public static BmpMessage Decode(byte[] data)
        {
            switch ((BmpMessageType)data[5])
            {
                case BmpMessageType.RouteMonitoring:
                    return new RouteMonitoring(data);
                case BmpMessageType.StatisticsReport:
                    return new StatisticsReport(data);
                case BmpMessageType.PeerDown:
                    return new PeerDownNotification(data);
                case BmpMessageType.PeerUp:
                    return new PeerUpNotification(data);
                case BmpMessageType.Initiation:
                    return new BmpInitiation(data);
                case BmpMessageType.Termination:
                    throw new NotImplementedException();
                default:
                    return null;
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

                // check bytes

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
