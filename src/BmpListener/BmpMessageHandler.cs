using BmpListener.Bmp;
using DotNetty.Transport.Channels;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace BmpListener
{
    public class BmpMessageHandler : SimpleChannelInboundHandler<BmpMessage>
    {
        Subject<BmpMessage> onMessageReceived = new Subject<BmpMessage>();

        public IObservable<BmpMessage> OnMessageReceived
        {
            get { return onMessageReceived.AsObservable(); }
        }
        
        public override void ExceptionCaught(IChannelHandlerContext context, Exception e) => context.CloseAsync();

        protected override void ChannelRead0(IChannelHandlerContext context, BmpMessage msg)
        {
            try
            {
                onMessageReceived.OnNext(msg);
            }
            catch (Exception ex)
            {
                onMessageReceived.OnError(ex);
            }
        }
    }
}
