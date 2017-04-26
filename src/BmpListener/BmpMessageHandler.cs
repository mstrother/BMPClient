using BmpListener.Bmp;
using DotNetty.Transport.Channels;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace BmpListener
{
    public class BmpMessageHandler : SimpleChannelInboundHandler<BmpMessage>
    {
        private static readonly Subject<BmpMessage> BmpMessageReceived = new Subject<BmpMessage>();

        public static IObservable<BmpMessage> MessageReceived => BmpMessageReceived;

        public override void ExceptionCaught(IChannelHandlerContext context, Exception e) => context.CloseAsync();

        protected override void ChannelRead0(IChannelHandlerContext context, BmpMessage msg)
        {
            try
            {
                BmpMessageReceived.OnNext(msg);
            }
            catch (Exception ex)
            {
                BmpMessageReceived.OnError(ex);
            }
        }
    }
}
