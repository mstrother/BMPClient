using BmpListener.Bmp;
using DotNetty.Transport.Channels;
using System;
using System.Diagnostics;

namespace BmpListener
{
    public class BmpMessageHandler : SimpleChannelInboundHandler<BmpMessage>
    {
        EventHandler<MessageReceivedEventArgs> MessageReceived;

        public BmpMessageHandler(EventHandler<MessageReceivedEventArgs> eventHandler)
        {
            MessageReceived = eventHandler;
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception e) => context.CloseAsync();

        protected override void ChannelRead0(IChannelHandlerContext context, BmpMessage msg)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(msg));
        }
    }
}
